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
    class LineOfOrdersViewModel : BaseViewModel
    {
       

        private Product _selectedProduct;
        private string _searchText;
        private int _quantity = 1;
        private double _sum = 0;
        //public ObservableCollection<Product> ProductList { get; }
        //public ObservableCollection<Product> SelectedProductList { get; set; }
        //public Command LoadItemsCommand { get; }
        public Command LoadItemCommand { get; }
        public Command SaveCommand { get; }
        public LineOfOrdersViewModel()
        {
            //ProductList = new ObservableCollection<Product>();
            //SelectedProductList = new ObservableCollection<Product>();
            //SelectedProductList.CollectionChanged += SelectedProductList_CollectionChanged;
            //LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            LoadItemCommand = new Command(async () => await ExecuteLoadItemCommand());
            SaveCommand = new Command(OnSave, ValidateSave);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
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
        //private async Task ExecuteLoadItemsCommand()
        //{
        //    IsBusy = true;

        //    try
        //    {
        //        ProductList.Clear();
        //        var items = await ProductRepo.GetItemsAsync(true);
        //        foreach (var item in items)
        //        {
        //            ProductList.Add(item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}
        public void OnAppearing()
        {
           // IsBusy = true;

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
        public string SearchText 
        {
            get => _searchText; 
            set
            {
                SetProperty(ref _searchText, value);
                if(!string.IsNullOrEmpty(value))
                    LoadItemCommand.Execute(null);

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
                Sum = this.Sum
            };

            await LinesRepo.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public int Quantity 
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
    }
}
