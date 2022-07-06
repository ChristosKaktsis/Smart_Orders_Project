using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    class BaseLineViewModel : BaseViewModel
    {
        private Product _selectedProduct;
        private string _searchText;
        private float _quantity = 1;
        private double _sum = 0;
        private bool _isFocused = true;
        private float _height = 0;
        private float _width = 0;
        private float _length = 0;
        private bool _isWEnabled;
        private bool _isLEnabled;
        private bool _isHEnabled;
        private bool _isWHLEnabled;
        private float _unit;
        private string _thisisa;
        public ObservableCollection<Product> ProductList { get; set; }
        //public ObservableCollection<Product> SelectedProductList { get; set; }
        public Command LoadItemsCommand { get; set; }
        //public Command LoadItemCommand { get; }
        public Command SaveCommand { get; set; }
        public Command ScannerCommand { get; set; }
        protected async void OnScannerClicked(object obj)
        {
            await Shell.Current.GoToAsync("BarCodeScanner");
        }
        protected async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                ProductList.Clear();
                //var items = await ProductRepo.GetItemsAsync(true);
                if (SearchText.Length == 13)
                {
                    var it = await ProductRepo.GetItemAsync(SearchText);
                    if (it != null)
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
                System.Diagnostics.Debug.WriteLine(ex);
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
                    Width = value.Width == 0 ? 1 : value.Width;
                    Height = value.Height == 0 ? 1 : value.Height;
                    Length = value.Length == 0 ? 1 : value.Length;
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
        protected async Task ExecuteLoadItemCommand()
        {
            IsBusy = true;

            try
            {
                var items = await ProductRepo.GetItemAsync(SearchText);
                SelectedProduct = items;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Σφάλμα!", "ExecuteLoadItemCommand \n" + ex.Message, "Οκ");
            }
            finally
            {
                IsBusy = false;
            }
        }
        public bool ValidateSave()
        {
            return SelectedProduct != null;
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
        protected virtual void CheckSum()
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
    }
}
