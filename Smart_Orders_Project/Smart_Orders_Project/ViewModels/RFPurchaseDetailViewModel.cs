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
    [QueryProperty(nameof(ProviderID), nameof(ProviderID))]
    public class RFPurchaseDetailViewModel : BaseViewModel
    {
        private string providerName;
        private RFPurchase rFPurchase;
        public ObservableCollection<RFPurchaseLine> LinesList { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command SelectProviderCommand { get; set; }
        public Command SelectProductCommand { get; set; }
        public RFPurchaseDetailViewModel()
        {
            InitializeModel();
            SelectProviderCommand = new Command(ExecuteSelectProviderCommand);
            SelectProductCommand = new Command(ExecuteSelectProductCommand);
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        private  void InitializeModel()
        {
            //view model lives after going to the next page 
            rFPurchase = new RFPurchase
            {
                Oid = Guid.NewGuid(),
                Lines = new List<RFPurchaseLine>(),
            };
            LinesList = new ObservableCollection<RFPurchaseLine>();
        }

        private async void ExecuteSelectProductCommand(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(Views.RFPurchaseLineSelectionPage)}?{nameof(RFPurchaseLinesViewModel.ItemId)}={rFPurchase.Oid}");
        }

        public string ProviderName
        {
            get
            {
                return providerName;
            }
            set
            {
                SetProperty(ref providerName, value);
            }
        }
        private async void ExecuteSelectProviderCommand()
        {
            await Shell.Current.GoToAsync(nameof(ProviderSelectionPage));
        }

        public string ProviderID
        {
            set
            {
                LoadProvider(value);
            }
        }

        public async Task ExecuteLoadItemsCommand() 
        {
            IsBusy = true;
            try
            {
                var items = await App.Database.GetPurchaseLineAsync();
                foreach(var item in items)
                {
                    LinesList.Add(item);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void LoadProvider(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;
            try
            {
                var provider = await ProviderRepo.GetItemAsync(value);
                ProviderName = provider.Name;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }
    }
}
