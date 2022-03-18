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
        private Storage selectedStorage;
        private string positionid, productID, errorMessage;
        private Position position;
        protected Product product;
        protected bool _IsPositionFocused, _IsProductFocused, productHasError, positionHasError;
        private int _Quantity;
        

        public ObservableCollection<Storage> StorageList { get; set; }
        public Command LoadStorageCommand { get; set; }
        
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

                //if (!value)
                //    SetProduct(ProductID);
            }
        }
        public Product Product
        {
            get { return product; }
            set
            {
                SetProperty(ref product, value);
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
        
    }
}
