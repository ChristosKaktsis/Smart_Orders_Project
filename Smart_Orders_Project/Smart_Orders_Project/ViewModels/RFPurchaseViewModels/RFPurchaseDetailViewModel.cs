using DevExpress.XamarinForms.CollectionView;
using SmartMobileWMS.Models;
using SmartMobileWMS.Repositories;
using SmartMobileWMS.Services;
using SmartMobileWMS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace SmartMobileWMS.ViewModels
{
    public class RFPurchaseDetailViewModel : BaseViewModel
    {
        private PurchaseLineRepository lineRepository = new PurchaseLineRepository();

        private RFPurchase rfPurchase;
        public ObservableCollection<RFPurchaseLine> LineCollection
        { get; } = new ObservableCollection<RFPurchaseLine>();
        public ObservableCollection<Provider> ProviderCollection
        { get; } = new ObservableCollection<Provider>();
        public ObservableCollection<Product> ProductCollection
        { get; } = new ObservableCollection<Product>();
        public Command OpenPopUp { get; }
        public Command SaveCommand { get; }
        public RFPurchaseDetailViewModel(RFPurchase rFPurchase = null)
        {
            OpenPopUp = new Command(() => { Provider_Popup_isOpen = !Provider_Popup_isOpen; });
            SaveCommand = new Command(async () => await SaveOrder());
            InitializeRFPurchase(rFPurchase);
        }

        private async void InitializeRFPurchase(RFPurchase rFPurchase)
        {
            this.rfPurchase = rFPurchase;
            if (this.rfPurchase == null)
            {
                this.rfPurchase = new RFPurchase
                {
                    Oid = Guid.NewGuid()
                };
                return;
            }
            Provider = this.rfPurchase.Provider;
            ProviderDoc = this.rfPurchase.ProviderDoc;
            await LoadLinesOfPurchase(this.rfPurchase.Oid.ToString());
        }

        private async Task LoadLinesOfPurchase(string id)
        {
            IsBusy = true;
            try
            {
                var items = await lineRepository.GetItemsAsync(id);
                items.ForEach(item =>
                {
                    LineCollection.Add(item);
                });
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            finally { IsBusy = false; }
        }

        public void OnAppearing()
        {
            LoadCartItems();
        }
        private bool loadedFromCart;
        private async void LoadCartItems()
        {
            try
            {
                if (!Data.Cart.CartItems.Any()) return;
                var answer = await Shell.Current.DisplayAlert(
                    "Φορτωση Ειδων απο το καλάθι?",
                    "Υπάρχουν είδη στο καλάθι. Θα θέλατε να γίνει φόρτωση των ειδών?",
                    "Αποδοχή", "Άκυρο");
                if (!answer) return;
                foreach (var item in Data.Cart.CartItems)
                    AddNewLine(item.Product);
                loadedFromCart = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", "Η φορτωση καλαθιού απέτυχε !", "ΟΚ");
            }
        }
        private Provider _Provider;
        public Provider Provider { 
            get => _Provider; 
            set
            {
                _Provider = value;
                DisplayProvider = value == null ? "Επιλογή προμηθευτή" : value.Name;
                OpenPopUp.Execute(null);
            }
        }
        public bool Provider_Popup_isOpen
        {
            get => _Provider_Popup_isOpen;
            set => SetProperty(ref _Provider_Popup_isOpen, value);
        }
        private string _DisplayProvider = "Επιλογή προμηθευτή";
        private bool _Provider_Popup_isOpen;

        public string DisplayProvider
        {
            get => _DisplayProvider;
            set => SetProperty(ref _DisplayProvider, value);
        }
        public string ProviderDoc { get; set; }
        public async Task GetProviders(string search)
        {
            IsBusy = true;
            try
            {
                if (string.IsNullOrWhiteSpace(search)) return;
                ProviderCollection.Clear();
                ProviderRepository repository = new ProviderRepository();
                var items = await repository.GetItemAsync(search);
                items.ForEach(item => { ProviderCollection.Add(item); });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", "Η αναζήτηση προμηθευτή απέτυχε", "ΟΚ");
            }
            finally { IsBusy = false; }
        }
        public async Task SearchProduct(string search)
        {
            IsBusy = true;
            try
            {
                ProductCollection.Clear();
                if (string.IsNullOrWhiteSpace(search)) return;
                var items = await productRepository.SearchItemsAsync(search);
                items.ForEach(item => { ProductCollection.Add(item); });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", "Η αναζήτηση είδους απέτυχε", "ΟΚ");
            }
            finally { IsBusy = false; }
        }
        private Product _SelectedProduct;

        public Product SelectedProduct
        {
            get { return _SelectedProduct; }
            set
            {
                _SelectedProduct = value;
                AddNewLine(value);
            }
        }
        public async Task GetProduct(string id)
        {
            IsBusy = true;
            try
            {
                if (string.IsNullOrWhiteSpace(id)) return;
                var item = await productRepository.GetItemAsync(id);
                if (item == null) await Shell.Current.DisplayAlert("", "Το είδος δεν βρέθηκε", "ΟΚ");
                AddNewLine(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", "Η αναζήτηση είδους απέτυχε", "ΟΚ");
            }
            finally { IsBusy = false; }
        }

        private void AddNewLine(Product item)
        {
            if (item == null) return;
            var newLine = new RFPurchaseLine
            {
                Oid = Guid.NewGuid(),
                Product = item,
                Quantity = 1,
                RFSalesOid = rfPurchase.Oid
            };
            LineCollection.Add(newLine);
        }
        public void DeleteLine(RFPurchaseLine item)
        {
            if (item == null) return;
            LineCollection.Remove(item);
        }

        private async Task SaveOrder()
        {
            try
            {
                IsBusy = true;
                RFPurchaseRepository repository = new RFPurchaseRepository();
                rfPurchase.Provider = Provider;
                rfPurchase.ProviderDoc = ProviderDoc;
                bool added = false;
                bool updated = false;
                if(!(updated = await repository.UpdateItem(rfPurchase)))
                    added = await repository.AddItem(rfPurchase);
                if (!updated && !added)
                {
                    await Shell.Current.DisplayAlert("Η αποθηκευση δεν ολοκληρώθηκε",
                        "Δεν ήταν δυνατή η αποθήκευση της παραγγελίας", "ΟΚ");
                    return;
                }
                await SaveLineOfOrder();
                //change position to the cart items 
                if (loadedFromCart) await Data.Cart.SavePosition(0);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex) { 
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Αποτυχία αποθήκευσης",
                        "Προέκυψε κάποιο σφάλμα στην αποθήκευση της Παραγγελίας", "ΟΚ");
            } finally { IsBusy = false; }
        }
        private async Task SaveLineOfOrder()
        {
            await lineRepository.DeleteItems(rfPurchase.Oid.ToString());
            foreach (var line in LineCollection)
                await lineRepository.AddItem(line);
        }
    }
}
