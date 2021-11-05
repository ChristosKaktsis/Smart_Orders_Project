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
        private int _quantity = 1;
        public ObservableCollection<Product> ProductList { get; }
        public ObservableCollection<Product> SelectedProductList { get; set; }
        public Command LoadItemsCommand { get; }
        
        public LineOfOrdersViewModel()
        {
            ProductList = new ObservableCollection<Product>();
            SelectedProductList = new ObservableCollection<Product>();
            SelectedProductList.CollectionChanged += SelectedProductList_CollectionChanged;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            
        }

        private void SelectedProductList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            if (e.NewItems != null)
            {
                foreach (Product newItem in e.NewItems)
                {
                    newItem.Quantity = Quantity;
                }
            }
             
            if (e.OldItems != null)
            {
                foreach (Product oldItem in e.OldItems)
                {
                    oldItem.Quantity = 0;          
                }
            }
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                ProductList.Clear();
                var items = await ProductRepo.GetItemsAsync(true);
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
            IsBusy = true;

        }
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                SetProperty(ref _selectedProduct, value);
                value.Quantity = Quantity;
            }
        }
        public int Quantity 
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }
    }
}
