using SmartMobileWMS.Models;
using SmartMobileWMS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    public class PositionBaseViewModel : BaseViewModel
    {
        private Storage selectedStorage;
        private string positionid, productID, errorMessage;
        private Position position;
        protected Product product;
        protected bool _IsPositionFocused, _IsProductFocused, productHasError, positionHasError;
        private int _Quantity;
        private RepositoryPalette repositoryPalette;

        public ObservableCollection<Storage> StorageList { get; set; }
        public Command LoadStorageCommand { get; set; }
        protected RepositoryPositionChange positionChange;
        public ObservableCollection<Product> PaletteContent { get; set; }
        public PositionBaseViewModel()
        {
            InitializeModel();
            LoadStorageCommand = new Command(async () => await ExecuteLoadStorageCommand());
            
        }
        private void InitializeModel()
        {
            StorageList = new ObservableCollection<Storage>();
            positionChange = new RepositoryPositionChange();
            repositoryPalette = new RepositoryPalette();
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
                var items = await RFStorageRepo.GetItemsAsync();
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
                if (value != null)
                    DisplayFounder = value.Name;
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
                var item = await ProductRepo.GetItemAsync(value);
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
                var item = await positionChange.GetProductFromPosition(position, product);
                if (item == null)
                    return result;
                result = (item.Quantity - quantity) >= 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
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
                var item = await RFPositionRepo.GetItemAsync(value);
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
                var result = await positionChange.PositionChange(Position, Product, Quantity, type);
                Console.WriteLine($"Saved? {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Shell.Current.DisplayAlert("Προσοχή!", $"Κάτι πήγε στραβά :{ex}", "Ok");
            }
        }
        //not good practice 
        //palette founders 
        private Palette palette;
        private string displayFounder;
        public bool IsPalette(string productID)
        {
            string pnumber = App.Current.Resources["PaletteNO"] as string;
            if (string.IsNullOrWhiteSpace(productID))
                return false;
            return productID.StartsWith(pnumber);
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
                var item = await repositoryPalette.GetPalette(sscc);
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
        public async Task LoadContent()
        {
            try
            {
                IsBusy = true;
                if (Palette == null)
                    return;
                PaletteContent.Clear();
                var items = await repositoryPalette.GetPaletteContent(Palette.Oid.ToString());
                foreach (var item in items)
                    PaletteContent.Add(item);
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
        public async Task SavePaletteAtPosition(int type)
        {
            try
            {
                foreach(var item in PaletteContent)
                {
                    var result = await positionChange.PositionChange(Position, item, item.Quantity, type, palette:Palette);
                    Console.WriteLine($"Saved? {result}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Shell.Current.DisplayAlert("Προσοχή!", $"Κάτι πήγε στραβά :{ex}", "Ok");
            }

        }
    }
}
