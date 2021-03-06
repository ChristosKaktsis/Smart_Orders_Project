using SmartMobileWMS.Models;
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
        public ObservableCollection<RFSales> RFSalesList { get; set; }
        public Command LoadItemsCommand { get; }
        public Command<RFSales> RFEdit { get; }
        public Command<RFSales> RFDone { get; }
        public OrdersViewModel()
        {
            RFSalesList = new ObservableCollection<RFSales>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddOrder = new Command(OnAddOrderClicked);
            RFEdit = new Command<RFSales>(OnRFEdit);
            RFDone = new Command<RFSales>(OnRFDone);
        }

        private async void OnRFDone(RFSales rfsale)
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Ολοκλήρωση?", "Θέλετε να ολοκληρωθεί η Παραγγελία ;", "Ναι", "Οχι");
            if(answer)
            {        
                try
                {
                    rfsale.Complete = true;
                    await RFSalesRepo.UpdateItemAsync(rfsale);
                    var r = await RepositoryPrint.GetPrinters();
                    string action = await App.Current.MainPage.DisplayActionSheet("Θέλετε να γίνει Εκτύπωση ?", "Άκυρο", null, r.ToArray());
                    if(action!= "Άκυρο")
                        RepositoryPrint.SendPrint(action, rfsale.RFCount);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex);
                    await Shell.Current.DisplayAlert("Σφάλμα!", "RFDone \n" + ex.Message, "Οκ");
                }
                IsBusy = true;
            }          
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
            await Shell.Current.GoToAsync(nameof(OrderDetailPage));
        }
    }
}
