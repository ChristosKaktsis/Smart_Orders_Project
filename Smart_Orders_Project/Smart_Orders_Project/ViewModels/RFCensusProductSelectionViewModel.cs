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
    
    class RFCensusProductSelectionViewModel : BaseViewModel
    {
        private string itemId;
        private Product _selectedProduct;
        private string _searchText;
        private float _quantity = 1;
        private bool _isFocused=true;
        private bool _isWHLEnabled;

        public ObservableCollection<Product> ProductList { get; }
        //public ObservableCollection<Product> SelectedProductList { get; set; }
        public Command LoadItemsCommand { get; }
        //public Command LoadItemCommand { get; }
        public Command SaveCommand { get; }
        public Command ScannerCommand { get; }
        public RFCensusProductSelectionViewModel()
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
        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                ProductList.Clear();               
                if (SearchText.Length == 13)
                {
                    var it = await ProductRepo.GetItemAsync(SearchText);
                    ProductList.Add(it);
                }
                else
                {
                    var items = await ProductRepo.GetItemsWithNameAsync(SearchText);

                    foreach (var item in items)
                    {
                        ProductList.Add(item);
                    }
                }
                //if its only one item in the list make it selected item
                if (ProductList.Count == 1)
                {
                    SelectedProduct = ProductList[0];
                    if (IsQuickOn)
                        SaveCommand.Execute(null);
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
                if (value != null)
                {
                    IsWHLEnabled = true;   
                }
                else
                {
                    IsWHLEnabled = false;
                }
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
                        //να μην εκτελεστει γιατι γινεται φιλτραρισμα στο UI 
                        //οταν εχει δυο και παραπανω λεξεις αναλαμβανει το UI
                        string[] subs = SearchText.Split(' ');
                        if (subs.Length > 1)
                            return;
                        //
                        LoadItemsCommand.Execute(null);
                    }
                }
            }
        }    
        public bool IsWHLEnabled
        {
            get => _isWHLEnabled;
            set
            {
                SetProperty(ref _isWHLEnabled, value);
            }
        }
        public string SearchText 
        {
            get => _searchText; 
            set
            {
                SetProperty(ref _searchText, value);         
            } 
        }
        private bool ValidateSave()
        {
            return SelectedProduct != null;
        }
        private async void OnSave()
        {
            RFCensus newItem = new RFCensus()
            {
                Oid = Guid.NewGuid(),
                Product = SelectedProduct,
                Quantity = (decimal)Quantity,
            };

            await RFCensusRepo.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public float Quantity 
        {
            get => _quantity;
            set 
            {
                SetProperty(ref _quantity, value);
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
