using SmartMobileWMS.Models;
using SmartMobileWMS.Repositories;
using SmartMobileWMS.Services;
using SmartMobileWMS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    class OrdersViewModel : BaseViewModel
    {
        public Command AddOrder { get; }
        public ObservableCollection<RFSale> RFSalesList { get; set; }
        public Command LoadItemsCommand { get; }
        public Command<RFSale> RFEdit { get; }
        public Command<RFSale> RFDone { get; }
        private RFSaleRepository saleRepository = new RFSaleRepository();
        public OrdersViewModel()
        {
            RFSalesList = new ObservableCollection<RFSale>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddOrder = new Command(OnAddOrderClicked);
            RFEdit = new Command<RFSale>(OnRFEdit);
            RFDone = new Command<RFSale>(OnRFDone);
        }

        private async void OnRFDone(RFSale rfsale)
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Ολοκλήρωση?", "Θέλετε να ολοκληρωθεί η Παραγγελία ;", "Ναι", "Οχι");
            if (!answer) return;
            try
            {
                rfsale.Complete = true;
                await saleRepository.UpdateItem(rfsale);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Σφάλμα!", "RFDone \n" + ex.Message, "Οκ");
            }
            IsBusy = true;
        }

        private async void OnRFEdit(RFSale rf)
        {
            if (rf == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(OrderDetailPage)}");
            await Shell.Current.Navigation.PushAsync(new OrderDetailPage(rf));
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                RFSalesList.Clear();
                var items = await saleRepository.GetItemsAsync();
                if (items == null) return;
                foreach (var item in items)
                    RFSalesList.Add(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Σφάλμα!", "ExecuteLoadItemsCommand \n"+ex.Message, "Οκ");
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
            await Shell.Current.Navigation.PushAsync(new OrderDetailPage());
        }
    }
}
