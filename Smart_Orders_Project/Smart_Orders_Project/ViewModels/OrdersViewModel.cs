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
        public Command<RFSales> RFEdit { get; }
        public OrdersViewModel()
        {
            RFSalesList = new ObservableCollection<RFSales>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddOrder = new Command(OnAddOrderClicked);
            RFEdit = new Command<RFSales>(OnRFEdit);
        }

        private async void OnRFEdit(RFSales rf)
        {
            if (rf == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(OrderDetailPage)}?{nameof(OrdersDetailViewModel.OrderOid)}={rf.Oid}");
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
