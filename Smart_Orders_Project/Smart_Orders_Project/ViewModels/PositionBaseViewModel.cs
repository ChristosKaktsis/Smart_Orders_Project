using Smart_Orders_Project.Models;
using Smart_Orders_Project.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    public class PositionBaseViewModel : BaseViewModel
    {
        public ObservableCollection<Storage> StorageList { get; set; }
        public Command LoadStorageCommand { get; set; }
        public Command<int> SavePositionCommand { get; set; }
        protected RepositoryPositionChange positionChange;
        public PositionBaseViewModel()
        {
            InitializeModel();
            LoadStorageCommand = new Command(async () => await ExecuteLoadStorageCommand());
            
            
        }
        private void InitializeModel()
        {
            StorageList = new ObservableCollection<Storage>();
            positionChange = new RepositoryPositionChange();
        }
        protected async void ExecuteSavePosition(int type)
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
        private Storage selectedStorage;
        private string positionid;
        private Position position;
        private string productID;
        private Product product;
        private bool _IsPositionFocused;
        private bool _IsProductFocused;
        private int _Quantity;
        private bool productHasError;
        private string errorMessage;
        private bool positionHasError;
        protected int im_ex;

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
                SetProperty(ref positionid, value);
                
            }
        }
        public string ProductID
        {
            get { return productID; }
            set
            {
                SetProperty(ref productID, value);
                
            }
        }
        public bool IsProductFocused
        {
            get { return _IsProductFocused; }
            set
            {
                SetProperty(ref _IsProductFocused, value);
                
                if (!value)
                    SetProduct(ProductID);
            }
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
        public Product Product
        {
            get { return product; }
            set
            {
                SetProperty(ref product, value);
                if (value != null && IsQuickOn)
                    SavePositionCommand.Execute(im_ex);
            }
        }
        private async void SetProduct(string value)
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
        
    }
}
