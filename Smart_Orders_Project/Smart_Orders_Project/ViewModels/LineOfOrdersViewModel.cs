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
        private double _height = 0;
        private double _width = 0;
        private double _length = 0;
        private bool _isWEnabled;
        private bool _isLEnabled;
        private bool _isHEnabled;

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
                    SelectedProduct = ProductList[0];
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
                    Width = value.Width==0 ? 1: value.Width;
                    Height = value.Height==0 ? 1: value.Height;
                    Length = value.Length==0 ? 1 : value.Length;
                    switch (value.Type)
                    {
                        case 0:
                            IsWEnabled = false;
                            IsLEnabled = false;
                            IsHEnabled = false;
                            break;
                        case 1:
                            IsWEnabled = false;
                            IsLEnabled = true;
                            IsHEnabled = false;
                            break;
                        case 2:
                            IsWEnabled = true;
                            IsLEnabled = true;
                            IsHEnabled = false;
                            break;
                        case 3:
                            IsWEnabled = true;
                            IsLEnabled = true;
                            IsHEnabled = true;
                            break;
                    }
                }
                else
                {
                    IsWEnabled = false;
                    IsLEnabled = false;
                    IsHEnabled = false;
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
                        LoadItemsCommand.Execute(null);
                    }
                }
               
            }
        }
        public bool IsWEnabled
        {
            get => _isWEnabled;
            set
            {
                SetProperty(ref _isWEnabled, value);
            }
        }
        public bool IsLEnabled
        {
            get => _isLEnabled;
            set
            {
                SetProperty(ref _isLEnabled, value);
            }
        }
        public bool IsHEnabled
        {
            get => _isHEnabled;
            set
            {
                SetProperty(ref _isHEnabled, value);
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
                Quantity = Quantity,
                Width = Width,
                Height = Height,
                Length = Length,
                Sum = Sum,
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
                CheckSum();
                //if (SelectedProduct != null)
                //{
                //    //Sum = value * SelectedProduct.Price;
                   
                //}
            } 
        }

        private void CheckSum()
        {
            if (SelectedProduct == null)
                return;

            if(Width==1 && Length == 1)
            {
                Sum = Quantity * SelectedProduct.Price;
            }
            else
            {
                var athr = ((Width * Length) * SelectedProduct.Price) / (SelectedProduct.Width * SelectedProduct.Length);
                Sum = Quantity * athr;
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
        public double Height
        {
            get => _height;
            set
            {
                SetProperty(ref _height, value);
            }
        }
        public double Width
        {
            get => _width;
            set
            {
                SetProperty(ref _width, value);
                CheckSum();
            }
        }
        public double Length
        {
            get => _length;
            set
            {
                SetProperty(ref _length, value);
                CheckSum();
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
