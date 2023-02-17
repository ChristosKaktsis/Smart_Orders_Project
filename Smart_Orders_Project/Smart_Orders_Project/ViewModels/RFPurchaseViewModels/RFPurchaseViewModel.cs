using SmartMobileWMS.Models;
using SmartMobileWMS.Repositories;
using SmartMobileWMS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    public class RFPurchaseViewModel : BaseViewModel
    {
        public Command AddOrder { get; }
        public ObservableCollection<RFPurchase> RFPurchaseList { get; }
        public Command LoadItemsCommand { get; }
        public Command<RFPurchase> RFEdit { get; }
        public Command<RFPurchase> RFDone { get; }
        private RFPurchaseRepository purchaseRepository = new RFPurchaseRepository();
        public RFPurchaseViewModel()
        {
            RFPurchaseList = new ObservableCollection<RFPurchase>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddOrder = new Command(OnAddOrderClicked);
            RFEdit = new Command<RFPurchase>(OnRFEdit);
            RFDone = new Command<RFPurchase>(OnRFDone);
        }
        private async void OnRFDone(RFPurchase purchase)
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Ολοκλήρωση?", "Θέλετε να ολοκληρωθεί η Αγορά ;", "Ναι", "Οχι");
            if (!answer) return;
            try
            {
                //complete the Rfpurchase
                purchase.Complete = true;
                await purchaseRepository.UpdateItem(purchase);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Σφάλμα!", "RFDone \n" + ex.Message, "Οκ");
            }
            IsBusy = true;
        }

        private async void OnRFEdit(RFPurchase rf)
        {
            if (rf == null)
                return;
            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.Navigation.PushAsync(new RFPurchaseDetailPage(rf));
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                RFPurchaseList.Clear();
                
                var items = await purchaseRepository.GetItemsAsync();
                foreach (var item in items)
                    RFPurchaseList.Add(item);
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
            IsBusy = true;
        }

        private async void OnAddOrderClicked(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new RFPurchaseDetailPage());
        }
    }
}
