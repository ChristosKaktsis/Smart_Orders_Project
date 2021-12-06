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
        private float _quantity = 1;
        private double _sum = 0;
        private bool _isFocused=true;
        private float _height = 0;
        private float _width = 0;
        private float _length = 0;
        private bool _isWEnabled;
        private bool _isLEnabled;
        private bool _isHEnabled;
        private bool _isWHLEnabled;
        private float _unit;
        private string _thisisa;

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
                    if(it!=null)
                        ProductList.Add(it);
                    else
                        await Shell.Current.DisplayAlert("Barcode!", "το είδος δεν βρέθηκε", "Οκ");
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
                await Shell.Current.DisplayAlert("Σφάλμα!", "ExecuteLoadItemsCommand \n" + ex.Message, "Οκ");
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
                            Height = 1;
                            ThisIsA = "Τετρ.Μέτρα";
                            break;
                        case 3:
                            IsWEnabled = true;
                            IsLEnabled = true;
                            IsHEnabled = true;
                            ThisIsA = "Κυβ.Μέτρα";
                            break;
                    }
                }
                else
                {
                    IsWEnabled = false;
                    IsLEnabled = false;
                    IsHEnabled = false;
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
                await Shell.Current.DisplayAlert("Σφάλμα!", "ExecuteLoadItemCommand \n" + ex.Message, "Οκ");
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
                Quantity = (decimal)Quantity,
                Width = (decimal)Width,
                Height = (decimal)Height,
                Length = (decimal)Length,
                Sum = Sum,
                RFSalesOid = Guid.Parse(ItemId)
            };

            await LinesRepo.AddItemAsync(newItem);

            await Shell.Current.GoToAsync("..");
            if (IsQuickOn)
            {
                await Shell.Current.GoToAsync($"{nameof(Views.LineOfOrdersSelectionPage)}?{nameof(LineOfOrdersViewModel.ItemId)}={ItemId}");
            }

            // This will pop the current page off the navigation stack
            //await Shell.Current.GoToAsync("..");
        }
        public float Quantity 
        {
            get => _quantity;
            set 
            {
                SetProperty(ref _quantity, value);
                CheckSum(); 
            } 
        }
        private void CheckSum()
        {
            if (SelectedProduct == null)
                return;

            float oldwidth = SelectedProduct.Width == 0 ? 1 : SelectedProduct.Width;
            float oldlength = SelectedProduct.Length == 0 ? 1 : SelectedProduct.Length;
            float oldheight = SelectedProduct.Height == 0 ? 1 : SelectedProduct.Height;

            switch (SelectedProduct.Type)
            {
                case 1:
                    oldwidth = 1;
                    oldheight = 1;
                    break;
                case 2:
                    oldheight = 1;
                    break;
            }
            float oldunit = (oldwidth * oldlength) * oldheight;
            float newunit = (Width * Length) * Height;

            if (SelectedProduct.Length == 0)
            {
                Sum = Quantity * SelectedProduct.Price;
            }
            else
            {
                Unit = newunit * Quantity;
                if (oldunit == 0)
                {
                    Sum = 0;
                }
                else
                {
                    var athr = (newunit * SelectedProduct.Price) / oldunit;
                    Sum = Quantity * athr;
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
        public float Height
        {
            get => _height;
            set
            {
                SetProperty(ref _height, value);
                CheckSum();
            }
        }
        public float Width
        {
            get => _width;
            set
            {
                SetProperty(ref _width, value);
                CheckSum();
            }
        }
        public float Length
        {
            get => _length;
            set
            {
                SetProperty(ref _length, value);
                CheckSum();
            }
        }
        public string ThisIsA
        {
            get => _thisisa;
            set
            {
                SetProperty(ref _thisisa, value);
            }
        }
        public float Unit
        {
            get => _unit;
            set
            {
                SetProperty(ref _unit, value);            
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
