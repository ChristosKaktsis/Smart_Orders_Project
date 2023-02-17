using SmartMobileWMS.Models;
using SmartMobileWMS.Network;
using SmartMobileWMS.Repositories;
using SmartMobileWMS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    class RFCensusViewModel : BaseViewModel
    {
        private ProductRepository productRepository = new ProductRepository();
        private CensusRepository censusRepository = new CensusRepository();
        private StorageRepository storageRepository = new StorageRepository();
        private PositionRepository positionRepository = new PositionRepository();
        public ObservableCollection<RFCensus> RFCensusList 
        { get; } = new ObservableCollection<RFCensus>();
        public ObservableCollection<Storage> StorageCollection 
        { get; } = new ObservableCollection<Storage>();
        public Command OpenPopUp { get; }
        public RFCensusViewModel()
        {
            OpenPopUp = new Command(() => { Popup_isOpen = !Popup_isOpen; });
        }
        private Storage _Storage;
        public Storage Storage { 
            get=>_Storage;
            set { 
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
                await LoadCensus();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", "Η αναζήτηση Θέσης απέτυχε", "ΟΚ");
            }
            finally { IsBusy = false; }
        }
        private async Task LoadCensus()
        {
            IsBusy = true;
            try
            {
                RFCensusList.Clear();
                var user = await UserRepo.GetUser();
                var items = await censusRepository.GetItemsAsync(
                    user.UserID.ToString(),Storage.Oid.ToString(),Position.Oid.ToString());
                if(items==null) return;
                foreach (var item in items)
                    RFCensusList.Add(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", "Η φόρτωση RFΑπογραφής απέτυχε", "Οκ");
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async void OnAppearing()
        {
            await LoadStorage();
        }
        public async Task GetProduct(string id)
        {
            IsBusy = true;
            try
            {
                if (string.IsNullOrWhiteSpace(id)) return;
                var item = await productRepository.GetItemAsync(id);
                if (item == null) await Shell.Current.DisplayAlert("", "Το είδος δεν βρέθηκε", "ΟΚ");
                await SaveCensus(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", "Η αναζήτηση είδους απέτυχε", "ΟΚ");
            }
            finally { IsBusy = false; }
        }

        private async Task SaveCensus(Product product)
        {
            if(product == null) return;
            IsBusy = true;
            try
            {
                var user = await UserRepo.GetUser();
                var item = new RFCensus
                {
                    Oid = Guid.NewGuid(),
                    Product = product,
                    Storage = Storage,
                    Position = Position,
                    UserCreator = user,
                    Quantity = 1,
                    CreationDate = DateTime.Now,
                };
                var result = await censusRepository.AddItem(item);
                AddToList(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", "Η Απογραφή δεν αποθηκεύτηκε", "ΟΚ");
            }
            finally { IsBusy = false; }
        }

        private void AddToList(RFCensus item)
        {
            if(item == null) return;
            RFCensusList.Add(item);
        }
    }
}
