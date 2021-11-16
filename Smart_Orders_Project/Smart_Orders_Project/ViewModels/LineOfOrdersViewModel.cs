using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class LineOfOrdersViewModel : BaseViewModel
    {

        private string itemId;
        private Product _selectedProduct;
        private string _searchText;
        private double _quantity = 1;
        private double _sum = 0;
        private bool _isFocused=true;

        public ObservableCollection<Product> ProductList { get; }
        //public ObservableCollection<Product> SelectedProductList { get; set; }
        public Command LoadItemsCommand { get; }
        //public Command LoadItemCommand { get; }
        public Command SaveCommand { get; }
        public Command ScannerCommand { get; }
        public LineOfOrdersViewModel()
        {
            ProductList = new ObservableCollection<Product>();
            //SelectedProductList = new ObservableCollection<Product>();
            //SelectedProductList.CollectionChanged += SelectedProductList_CollectionChanged;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            //LoadItemCommand = new Command(async () => await ExecuteLoadItemCommand());
            SaveCommand = new Command(OnSave, ValidateSave);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            ScannerCommand = new Command(OnScannerClicked);
        }

        private async void OnScannerClicked(object obj)
        {
            await Shell.Current.GoToAsync("BarCodeScanner");
        }

        //private void SelectedProductList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{

        //    if (e.NewItems != null)
        //    {
        //        foreach (Product newItem in e.NewItems)
        //        {
        //            newItem.Quantity = Quantity;
        //        }
        //    }

        //    if (e.OldItems != null)
        //    {
        //        foreach (Product oldItem in e.OldItems)
        //        {
        //            oldItem.Quantity = 0;          
        //        }
        //    }
        //}
        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                ProductList.Clear();
                //var items = await ProductRepo.GetItemsAsync(true);
                var items = await ProductRepo.GetItemsWithNameAsync(SearchText);
                foreach (var item in items)
                {
                    ProductList.Add(item);
                }
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
        public void OnAppearing()
        {
            IsBusy = false;
            SearchText = BarCode;
        }
       
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                SetProperty(ref _selectedProduct, value);
                Quantity = 1; 
            }
        }
        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                SetProperty(ref _isFocused, value);
                if (!value)
                {
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        LoadItemsCommand.Execute(null);
                    }
                }
               
            }
        }
        public string SearchText 
        {
            get => _searchText; 
            set
            {
                SetProperty(ref _searchText, value);
                //if (!string.IsNullOrEmpty(value))
                //{                   
                //        LoadItemsCommand.Execute(null);
                //}
                    

            } 
        }
        private async Task ExecuteLoadItemCommand()
        {
            IsBusy = true;

            try
            {
                var items = await ProductRepo.GetItemAsync(SearchText);
                SelectedProduct = items;
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
        private bool ValidateSave()
        {
            return SelectedProduct != null;
        }
        private async void OnSave()
        {
            LineOfOrder newItem = new LineOfOrder()
            {
                Oid = Guid.NewGuid(),
                Product = SelectedProduct,
                Quantity = this.Quantity,
                Sum = this.Sum,
                RFSalesOid = Guid.Parse(ItemId)
            };

            await LinesRepo.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public double Quantity 
        {
            get => _quantity;
            set 
            {
                SetProperty(ref _quantity, value);
                if (SelectedProduct != null)
                {
                    Sum = value * SelectedProduct.Price;
                }
            } 
        }
        public double Sum
        {
            get => _sum;
            set
            {
                SetProperty(ref _sum, value);

            }
        }
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
            }
        }
    }
}
