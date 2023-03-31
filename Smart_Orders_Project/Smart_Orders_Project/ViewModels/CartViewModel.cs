using SmartMobileWMS.Data;
using SmartMobileWMS.Models;
using SmartMobileWMS.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        private StorageRepository storageRepository = new StorageRepository();
        public ObservableCollection<PositionChange> ProductCollection
        { get; } = Cart.CartItems;
        public ObservableCollection<Storage> StorageCollection
        { get; } = new ObservableCollection<Storage>();
        public Command OpenPopUp { get; }
        public CartViewModel()
        {
            OpenPopUp = new Command(() => { Popup_isOpen = !Popup_isOpen; });
        }
        private Storage _Storage;
        public Storage Storage
        {
            get => _Storage;
            set
            {
                _Storage = value;
                DisplayStorage = value == null ? "Αποθηξευτικός Χώρος" : value.Description;
                OpenPopUp.Execute(null);
            }
        }
        public Position Position { get; set; }
        private bool _Popup_isOpen;
        public bool Popup_isOpen
        {
            get => _Popup_isOpen;
            set => SetProperty(ref _Popup_isOpen, value);
        }
        private string _DisplayStorage = "Αποθηκευτικός Χώρος";

        public string DisplayStorage
        {
            get => _DisplayStorage;
            set => SetProperty(ref _DisplayStorage, value);
        }
        private async Task LoadStorage()
        {
            IsBusy = true;
            try
            {
                StorageCollection.Clear();
                var items = await storageRepository.GetItemsAsync();
                foreach (var item in items)
                    StorageCollection.Add(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", "Δεν ήταν δυνατή η φόρτοση των αποθηκευτικών χώρων", "Οκ");
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task GetPosition(string id)
        {
            IsBusy = true;
            try
            {
                if (string.IsNullOrWhiteSpace(id)) return;
                var item = await positionRepository.GetItemAsync(id);
                if (item == null) await Shell.Current.DisplayAlert("", "Η Θέση δεν βρέθηκε", "ΟΚ");
                Position = item;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", "Η αναζήτηση Θέσης απέτυχε", "ΟΚ");
            }
            finally { IsBusy = false; }
        }
        public async void OnAppearing()
        {
            await LoadStorage();
        }
        public async Task GetProduct(string id)
        {
            if(Position == null)
            {
                await Shell.Current.DisplayAlert("", "Θα πρέπει πρώτα να υπάρχει θέση για να εισάγετε είδος", "ΟΚ");
                return;
            }
            IsBusy = true;
            try
            {
                if (string.IsNullOrWhiteSpace(id)) return;
                var item = await productRepository.GetItemAsync(id);
                if (item == null) await Shell.Current.DisplayAlert("", "Το είδος δεν βρέθηκε", "ΟΚ");
                item.Quantity = 1;
                AddToCart(item);
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", "Η αναζήτηση είδους απέτυχε", "ΟΚ");
            }
            finally { IsBusy = false; }
        }

        private void AddToCart(Product item)
        {
            if (item == null) return;
            if (item.SN)
                if (Cart.CartItems.Where(c => c.Product.Oid == item.Oid).Any()) {
                    NotifySNNotValid();
                    return; }
            Cart.CartItems.Add(new PositionChange
            {
                Quantity = item.Quantity,
                Product = item,
                Position = Position
            });
        }

        public void RemoveItem(PositionChange item)
        {
            if (item == null) return;
            Data.Cart.CartItems.Remove(item);
        }
    }
}
