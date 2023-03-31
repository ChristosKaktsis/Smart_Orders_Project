using SmartMobileWMS.Constants;
using SmartMobileWMS.Models;
using SmartMobileWMS.Repositories;
using SmartMobileWMS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    public class PositionBaseViewModel : BaseViewModel
    {
        private StorageRepository storageRepository = new StorageRepository();
        private Storage selectedStorage;
        private string positionid, productID, errorMessage;
        private Position position;
        protected Product product;
        protected bool _IsPositionFocused, _IsProductFocused, productHasError, positionHasError;
        private int _Quantity;
        private PaletteRepository repositoryPalette;
        protected PositionChangeRepository positionChangeRepository = new PositionChangeRepository();

        public ObservableCollection<Storage> StorageList { get; set; }
        public Command LoadStorageCommand { get; set; }
        public ObservableCollection<Product> PaletteContent { get; set; }
        public ObservableCollection<Product> Cart 
        { get; } = new ObservableCollection<Product>();
        public PositionBaseViewModel()
        {
            InitializeModel();
            LoadStorageCommand = new Command(async () => await ExecuteLoadStorageCommand());
        }
        private void InitializeModel()
        {
            StorageList = new ObservableCollection<Storage>();
            repositoryPalette = new PaletteRepository();
            PaletteContent = new ObservableCollection<Product>();
        }
        public void OnAppearing()
        {
            LoadStorageCommand.Execute(null);
        }
        private async Task ExecuteLoadStorageCommand()
        {
            IsBusy = true;
            try
            {
                StorageList.Clear();
                var items = await storageRepository.GetItemsAsync();
                foreach (var item in items)
                    StorageList.Add(item);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public Storage SelectedStorage
        {
            get { return selectedStorage; }
            set { SetProperty(ref selectedStorage, value); }
        }
        public string PositionID
        {
            get { return positionid; }
            set 
            { 
                positionid = value; 
            }
        }
        public string ProductID
        {
            get { return productID; }
            set
            {
                productID=value;
            }
        }
        public bool IsProductFocused
        {
            get { return _IsProductFocused; }
            set
            {
                SetProperty(ref _IsProductFocused, value);
            }
        }
        public Product Product
        {
            get { return product; }
            set
            {
                SetProperty(ref product, value);
                DisplayFounder = value != null? value.Name:string.Empty;
            }
        }
        public async Task SetProduct(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Product = null;
                return;
            }

            IsBusy = true;
            try
            {
                var item = await productRepository.GetItemAsync(value);
                Product = item;

                ProductHasError = Product == null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task<bool> AnyProductLeft(string position, string product, int quantity)
        {
            bool result = false;
            try
            {
                var item = await productRepository.GetItemAsync(product,position);
                if (item == null)
                    return result;
                result = (item.Quantity - quantity) >= 0;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return result;
        }
        public async Task<bool> ProductExistInPosition(string position, string product)
        {
            var item = await productRepository.GetItemAsync(product,position);
            if (item == null)
                return false;
            return (item.Quantity > 0 );
        }
        public bool ProductHasError
        {
            get { return productHasError; }
            set
            {
                SetProperty(ref productHasError, value);
                if (value)
                    ErrorMessage = "Το είδος δεν βρέθηκε";
            }
        }
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                SetProperty(ref errorMessage, value);

            }
        }
        public bool IsPositionFocused
        {
            get { return _IsPositionFocused; }
            set
            {
                SetProperty(ref _IsPositionFocused, value);
                if(!value)
                    SetPosition(PositionID);
            }
        }
        public Position Position
        {
            get { return position; }
            set
            {
                SetProperty(ref position, value);
            }
        }
        public bool PositionHasError
        {
            get { return positionHasError; }
            set
            {
                SetProperty(ref positionHasError, value);
                if (value)
                    ErrorMessage = "Η θέση δεν βρέθηκε";
            }
        }
        private async void SetPosition(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            IsBusy = true;
            try
            {
                var item = await positionRepository.GetItemAsync(value);
                Position = item;
                PositionHasError = Position == null;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public int Quantity
        {
            get { return _Quantity = IsQuickOn ? 1 : _Quantity; }
            set
            {
                SetProperty(ref _Quantity, value);
            }
        }
        public async Task ExecuteSavePosition(int type)
        {
            try
            {
                var item = new PositionChange
                {
                    Position = Position,
                    Product = Product,
                    Quantity = Quantity,
                    Type = type
                };
                var result = await positionChangeRepository.AddItem(item);
                Product.Quantity = Quantity;
                Cart.Add(Product);
                Product = null;
                Debug.WriteLine($"Saved? {result}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", $"Η ενέργεια δεν πραγματοποιήθηκε", "Ok");
            }
        }
        //not good practice 
        //palette founders 
        private Palette palette;
        private string displayFounder;
        public bool IsPalette(string productID)
        {
            if (string.IsNullOrWhiteSpace(productID))
                return false;
            if(SSCCDigits<=0) return false;
            return productID.Length >= SSCCDigits;
        }
        public Palette Palette
        {
            get => palette;
            set
            {
                SetProperty(ref palette, value);
                if (value != null)
                    DisplayFounder = value.Description;
            }
        }
        public string DisplayFounder
        {
            get => displayFounder;
            set => SetProperty(ref displayFounder, value);
        }
        public async Task FindPalette(string sscc)
        {
            try
            {
                IsBusy = true;
                ProductHasError = false;
                var item = await repositoryPalette.GetItemAsync(SSCC.GetSSCC(sscc,SSCCStart,SSCCEnd));
                if (ProductHasError = item == null)
                {
                    ErrorMessage = "Η παλέτα δεν βρέθηκε";
                }
                Palette = item;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void LoadContent()
        {
            try
            {
                IsBusy = true;
                if (Palette == null)
                    return;
                PaletteContent.Clear();
                var items = Palette.Products;
                foreach (var item in items)
                    PaletteContent.Add(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task SavePaletteAtPosition(int type)
        {
            try
            {
                foreach(var item in PaletteContent)
                {
                    var pChange = new PositionChange
                    {
                        Position = Position,
                        Product = item,
                        Quantity = item.Quantity,
                        Type = type,
                        Palette = Palette
                    };
                    Cart.Add(item);
                    var result = await positionChangeRepository.AddItem(pChange);
                    Debug.WriteLine($"Saved? {result}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", $"Η ενέργεια δεν πραγματοποιήθηκε", "Ok");
            }
        }
    }
}
