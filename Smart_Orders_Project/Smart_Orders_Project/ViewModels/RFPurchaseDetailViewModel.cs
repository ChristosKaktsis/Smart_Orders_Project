using Smart_Orders_Project.Models;
using Smart_Orders_Project.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    [QueryProperty(nameof(ProviderID), nameof(ProviderID))]
    [QueryProperty(nameof(RFPurchaseID), nameof(RFPurchaseID))]
    public class RFPurchaseDetailViewModel : BaseViewModel
    {
        private string providerName = "Επιλογή Προμηθευτή";
        private RFPurchase rfPurchase;
        private string providerDoc;

        public ObservableCollection<RFPurchaseLine> LinesList { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command SelectProviderCommand { get; set; }
        public Command SelectProductCommand { get; set; }
        public Command SaveCommand { get; set; }
        public RFPurchaseDetailViewModel()
        {
            InitializeModel();
            SelectProviderCommand = new Command(ExecuteSelectProviderCommand);
            SelectProductCommand = new Command(ExecuteSelectProductCommand);
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SaveCommand = new Command(ExecuteSaveCommand, ValidateSave);
            this.PropertyChanged +=
               (_, __) => SaveCommand.ChangeCanExecute();
        }
        private bool ValidateSave()
        {
            return ProviderName != "Επιλογή Προμηθευτή" && LinesList.Any();
        }
        private async void ExecuteSaveCommand()
        {
            try
            {
                AddLinesToPurchase();
                //check if exist
                var exist = await App.Database.GetPurchaseAsync(rFPurchase.Oid.ToString());
                if (exist == null)
                {
                    await App.Database.AddPurchaseAsync(rFPurchase);
                }
                else
                    //update
                    await RFPurchaseRepo.UpdateItemAsync(rFPurchase);

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            await Shell.Current.GoToAsync("..");
        }

        private void AddLinesToPurchase()
        {
            foreach (var item in LinesList)
                if(!rFPurchase.Lines.Contains(item))
                    rFPurchase.Lines.Add(item);
        }

        private  void InitializeModel()
        {
            //view model lives after going to the next page
            //
            LinesList = new ObservableCollection<RFPurchaseLine>();

        }
        public RFPurchase rFPurchase 
        { 
            get 
            { 
                if(rfPurchase== null)
                {
                    rfPurchase = new RFPurchase
                    {
                        Oid = Guid.NewGuid(),
                        Lines = new List<RFPurchaseLine>(),
                        CreationDate = DateTime.Now,
                    };
                }
                return rfPurchase;
            }
            set
            {
                rfPurchase = value;
                AddLinesToData(value);
                ProviderDoc = value.ProviderDoc;
                ProviderName = value.Provider.Name;
            } 
        }

        private  void AddLinesToData(RFPurchase value)
        {
            if (value == null)
                return;
            foreach (var item in value.Lines)
                if (!LinesList.Contains(item))
                    LinesList.Add(item);
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
        public string ProviderDoc
        {
            get
            {
                return providerDoc;
            }
            set
            {
                SetProperty(ref providerDoc, value);
                if (!string.IsNullOrEmpty(value))
                    rFPurchase.ProviderDoc = value;
            }
        }
        public  void OnAppearing()
        {
            IsBusy = true;
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
        public string RFPurchaseID
        {
            set
            {
                LoadRFPurchase(value);
            }
        }

        private async void LoadRFPurchase(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;
            try
            {
                rFPurchase = await App.Database.GetPurchaseAsync(value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task ExecuteLoadItemsCommand() 
        {
            IsBusy = true;
            try
            {
                
                var items = await App.Database.GetPurchaseLineAsync();
                foreach(var item in items)
                {
                    if(!LinesList.Contains(item))
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
                AddProviderToPurchase(provider);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        private void AddProviderToPurchase(Provider provider)
        {
            if (provider == null)
                return;

            rFPurchase.Provider = provider;
        }
    }
}
