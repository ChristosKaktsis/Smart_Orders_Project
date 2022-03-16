using Smart_Orders_Project.Models;
using Smart_Orders_Project.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    public class RFPurchaseViewModel : BaseViewModel
    {
        public Command AddOrder { get; }
        public ObservableCollection<RFPurchase> RFPurchaseList { get; set; }
        public Command LoadItemsCommand { get; }
        public Command<RFPurchase> RFEdit { get; }
        public Command<RFPurchase> RFDone { get; }
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
            if (answer)
            {
                try
                {
                    //complete the Rfpurchase
                    purchase.Complete = true;
                    var result = await RFPurchaseRepo.UpdateItemAsync(purchase);
                    Console.WriteLine($"Complete ?{result}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    await Shell.Current.DisplayAlert("Σφάλμα!", "RFDone \n" + ex.Message, "Οκ");
                }
                IsBusy = true;
            }
        }

        private async void OnRFEdit(RFPurchase rf)
        {
            if (rf == null)
                return;

            await App.Database.AddPurchaseAsync(rf);
            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(RFPurchaseDetailPage)}?{nameof(RFPurchaseDetailViewModel.RFPurchaseID)}={rf.Oid}");
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                RFPurchaseList.Clear();
                
                var items = await RFPurchaseRepo.GetItemsAsync();
                foreach (var item in items)
                {
                    RFPurchaseList.Add(item);
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
            IsBusy = true;
            ClearData();//clear cart
        }

        private async void ClearData()
        {
            await App.Database.DeletePurchaseLinesAsync();
        }

        private async void OnAddOrderClicked(object obj)
        {
            await Shell.Current.GoToAsync(nameof(RFPurchaseDetailPage));
        }
    }
}
