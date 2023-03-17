using SmartMobileWMS.Models;
using SmartMobileWMS.Repositories;
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
using Xamarin.Forms.Shapes;

namespace SmartMobileWMS.ViewModels
{
    class OrdersDetailViewModel : BaseViewModel
    {
        private LineRepository lineRepository = new LineRepository();
        private RFSale RFSale;
        private Counter counter;
        public OrdersDetailViewModel(RFSale rFSale = null)
        {
            OpenPopUp = new Command(() => { Customer_Popup_isOpen = !Customer_Popup_isOpen; });
            SaveCommand = new Command(async () => await SaveOrder());
            InitializeRFSale(rFSale);
        }

        private async void InitializeRFSale(RFSale rFSale)
        {
            RFSale = rFSale;
            if (RFSale == null)
            {
                RFSale = new RFSale
                {
                    Oid = Guid.NewGuid()
                };
                return;
            }
            Customer = RFSale.Customer;
            Reciever = RFSale.Reciever;
            Title = RFSale.RFCount;
            await LoadLinesOfSale(RFSale.Oid.ToString());
        }

        private async Task LoadLinesOfSale(string id)
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
            catch(Exception ex) { Debug.WriteLine(ex); }
            finally { IsBusy = false; }
        }
        public ObservableCollection<LineOfOrder> LineCollection 
        { get; } = new ObservableCollection<LineOfOrder>();
        public ObservableCollection<Reciever> RecieverCollection 
        { get; } = new ObservableCollection<Reciever>();
        public ObservableCollection<Customer> CustomerCollection
        { get; } = new ObservableCollection<Customer>();
        public ObservableCollection<Product> ProductCollection 
        { get; } = new ObservableCollection<Product>();
        private Customer _Customer;
        public Customer Customer { 
            get=>_Customer;
            set { 
                _Customer = value;
                DisplayCustomer = value==null? "Επιλογή Πελάτη" : value.Name;
                OpenPopUp.Execute(null);
            }
        }
        public Reciever Reciever { get; set; }
        public bool Customer_Popup_isOpen { 
            get => _Customer_Popup_isOpen; 
            set => SetProperty(ref _Customer_Popup_isOpen, value); 
        }
        private string _DisplayCustomer = "Επιλογή Πελάτη";
        private bool _Customer_Popup_isOpen;
        public string DisplayCustomer
        {
            get =>_DisplayCustomer; 
            set =>SetProperty(ref _DisplayCustomer, value);
        }
        public Command OpenPopUp { get; }
        public Command SaveCommand { get; }
        public void OnAppearing()
        {
            LoadRecievers();
            GetCounter();
            LoadCartItems();
        }
        private bool loadedFromCart;
        private async void LoadCartItems()
        {
            try
            {
                if(!Data.Cart.CartItems.Any()) return;
                var answer =  await Shell.Current.DisplayAlert(
                    "Φορτωση Ειδων απο το καλάθι?", 
                    "Υπάρχουν είδη στο καλάθι. Θα θέλατε να γίνει φόρτωση των ειδών?", 
                    "Αποδοχή", "Άκυρο");
                if(!answer) return;
                foreach(var item in Data.Cart.CartItems)
                    AddNewLine(item.Product);
                loadedFromCart = true;
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", "Η φορτωση καλαθιού απέτυχε !", "ΟΚ");
            }
        }

        private async void GetCounter()
        {
            try
            {
                CounterRepository repository = new CounterRepository();
                var item = await repository.GetItemAsync();
                if (item != null)
                {
                    counter = item;
                    return;
                }
                counter = new Counter { Value = 0 };
                await repository.AddItem(counter);
            }
            catch(Exception ex) { Debug.WriteLine(ex); }
        }
        private async void LoadRecievers()
        {
            try
            {
                RecieverCollection.Clear();
                RecieverRepository repository = new RecieverRepository();
                var items = await repository.GetItemsAsync();
                foreach (var item in items)
                    RecieverCollection.Add(item);
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }
        private bool _ProductPoupIsOpen;
        public bool ProductPoupIsOpen { get=> _ProductPoupIsOpen; 
            set=>SetProperty(ref _ProductPoupIsOpen,value); }
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
            set { _SelectedProduct = value;
                AddNewLine(value);
            }
        }

        public async Task GetProduct(string id) {
            IsBusy = true;
            try
            {
                if (string.IsNullOrWhiteSpace(id)) return;
                var item = await productRepository.GetItemAsync(id);
                if(item == null) await Shell.Current.DisplayAlert("", "Το είδος δεν βρέθηκε", "ΟΚ");
                AddNewLine(item);
            }
            catch(Exception ex) { 
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", "Η αναζήτηση είδους απέτυχε", "ΟΚ");
            }
            finally { IsBusy = false; }
        }
        public async Task GetCustomers(string search)
        {
            IsBusy = true;
            try
            {
                if(string.IsNullOrWhiteSpace(search)) return;
                CustomerCollection.Clear();
                CustomerRepository repository = new CustomerRepository();
                var items = await repository.GetItemAsync(search);
                items.ForEach(item =>{CustomerCollection.Add(item);});
            }
            catch(Exception ex) { 
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", "Η αναζήτηση πελάτη απέτυχε", "ΟΚ");
            }
            finally { IsBusy = false; }
        }
        private void AddNewLine(Product item)
        {
            if (item == null) return;
            var newLine = new LineOfOrder
            {
                Oid = Guid.NewGuid(),
                Product = item,
                Quantity = 1,
                RFSalesOid = RFSale.Oid
            };
            LineCollection.Add(newLine);
        }
        public void DeleteLine(LineOfOrder item)
        {
            if (item == null) return;
            LineCollection.Remove(item);
        }
        private async Task SaveOrder()
        {
            IsBusy = true;
            try
            {
                RFSale.Customer = Customer;
                RFSale.Reciever = Reciever;
                if (string.IsNullOrEmpty(RFSale.RFCount))
                {
                    counter.Value++;
                    RFSale.RFCount = "RF" + counter.Value.ToString().PadLeft(9, '0');
                }
                RFSaleRepository saleRepository = new RFSaleRepository();
                bool added = false;
                bool updated = false;
                if (!(updated = await saleRepository.UpdateItem(RFSale)))
                    added = await saleRepository.AddItem(RFSale);
                if (!updated && !added)
                {
                    await Shell.Current.DisplayAlert("Η αποθηκευση δεν ολοκληρώθηκε",
                        "Δεν ήταν δυνατή η αποθήκευση της παραγγελίας", "ΟΚ");
                    return;
                }
                await SaveLineOfOrder();
                if (added)
                    await UpdateCounter(counter);
                //change position to the cart items 
                if (loadedFromCart) await Data.Cart.SavePosition(1);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Αποτυχία αποθήκευσης",
                        "Προέκυψε κάποιο σφάλμα στην αποθήκευση της Παραγγελίας", "ΟΚ");
            }
            finally { IsBusy = false; }
        }

        private async Task UpdateCounter(Counter counter)
        {
            CounterRepository counterRepository = new CounterRepository();
            await counterRepository.UpdateItem(counter);
        }

        private async Task SaveLineOfOrder()
        {
            
            await lineRepository.DeleteItems(RFSale.Oid.ToString());
            foreach (var line in LineCollection)
                await lineRepository.AddItem(line);
        }
    }
}
