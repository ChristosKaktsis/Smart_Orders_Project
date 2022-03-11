using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    public class PositionImportViewModel : BaseViewModel
    {
        public ObservableCollection<Storage> StorageList { get; set; }
        public Command LoadStorageCommand { get; set; }
        public PositionImportViewModel()
        {
            InitializeModel();
            LoadStorageCommand = new Command(async () => await ExecuteLoadStorageCommand());
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
        public Product Product
        {
            get { return product; }
            set
            {
                SetProperty(ref product, value);
            }
        }
        private async void SetProduct(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            IsBusy = true;
            try
            {
                var item = await ProductRepo.GetItemAsync(value);
                Product = item;
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
        private async void SetPosition(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            IsBusy = true;
            try
            {
                var item = await RFPositionRepo.GetItemAsync(value);
                Position = item;
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

        private void InitializeModel()
        {
            StorageList = new ObservableCollection<Storage>();
        }
    }
}
