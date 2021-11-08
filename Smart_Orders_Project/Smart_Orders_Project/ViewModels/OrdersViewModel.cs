using Smart_Orders_Project.Models;
using Smart_Orders_Project.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    class OrdersViewModel : BaseViewModel
    {
        public Command AddOrder { get; }
        public ObservableCollection<RFSales> RFSalesList { get; set; }
        public Command LoadItemsCommand { get; }
        public OrdersViewModel()
        {
            RFSalesList = new ObservableCollection<RFSales>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddOrder = new Command(OnAddOrderClicked);        
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                RFSalesList.Clear();
                var items = await RFSalesRepo.GetItemsAsync(true);
                foreach (var item in items)
                {
                    RFSalesList.Add(item);
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
        private async void OnAddOrderClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync(nameof(OrderDetailPage));
        }
    }
}
