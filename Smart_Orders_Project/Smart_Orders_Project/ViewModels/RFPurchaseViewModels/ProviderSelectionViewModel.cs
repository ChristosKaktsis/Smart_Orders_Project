using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    public class ProviderSelectionViewModel : BaseViewModel
    {
        private string search;

        public Command<Provider> AddProviderCommand { get; set; }
        public ObservableCollection<Provider> ProviderList { get; set; }
        public ProviderSelectionViewModel()
        {
            AddProviderCommand = new Command<Provider>(ExecuteAddProvider);
            ProviderList = new ObservableCollection<Provider>();
        }
        public string Search
        {
            get => search;
            set
            {
                
                LoadProviders(value);
            }
        }
        private async void LoadProviders(string search)
        {
            IsBusy = true;

            try
            {
                if (string.IsNullOrWhiteSpace(search))
                    return;

                ProviderList.Clear();

                var items = await ProviderRepo.GetItemsAsync();
                foreach (var item in items)
                {
                    ProviderList.Add(item);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Σφάλμα!", "LoadProviders \n" + ex.Message, "Οκ");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async void ExecuteAddProvider(Provider obj)
        {
            //null check on page Model
            await Shell.Current.GoToAsync($"..?{nameof(RFPurchaseDetailViewModel.ProviderID)}={obj.Oid}");
        }

    }
}
