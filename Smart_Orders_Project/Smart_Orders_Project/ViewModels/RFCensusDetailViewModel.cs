using Smart_Orders_Project.Models;
using Smart_Orders_Project.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    class RFCensusDetailViewModel : BaseViewModel
    {
        private float _quantity;
        private bool _isAddEnabled = true;
        private string _searchText;
        private Position _selectedPosition;
        private bool _isFocused;
        private bool _isRunning;

        public ObservableCollection<Storage> StorageList { get; set; }
        public ObservableCollection<RFCensus> RFCensusList { get; set; }
        public Command LoadItemsCommand { get; }
        public Command LoadRFPositionCommand { get; }
        public Command AddLine { get; }
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }
        public RFCensusDetailViewModel()
        {
            StorageList = new ObservableCollection<Storage>();
            RFCensusList = new ObservableCollection<RFCensus>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            LoadRFPositionCommand = new Command(async () => await ExecuteLoadRFPositionCommand());
            AddLine = new Command(OnAddLineClicked);
            DeleteCommand = new Command<RFCensus>(OnDeletePressed);
        }
        public string SearchPositionText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                
            }
        }
        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                SetProperty(ref _selectedPosition, value);
            }
        }
        private async Task ExecuteLoadRFPositionCommand()
        {
            try
            {
                IsRunning = true;
                SelectedPosition = await RFPositionRepo.GetItemAsync(SearchPositionText);
                if (SelectedPosition == null)
                    await Shell.Current.DisplayAlert("Προσοχή !", "Η θέση δεν βρέθηκε", "ΟΚ");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsRunning = false;
            }
        }
        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                SetProperty(ref _isFocused, value);
                if (!value)
                {
                    if (!string.IsNullOrEmpty(SearchPositionText))
                    {    
                        LoadRFPositionCommand.Execute(null);
                    }
                }

            }
        }
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                StorageList.Clear();
                var items = await RFStorageRepo.GetItemsAsync(true);
                foreach (var item in items)
                {
                    StorageList.Add(item);
                }
                //not sure about that
                var items2 = await RFCensusRepo.GetItemsAsync(true);
                foreach (var item in items2)
                {
                    RFCensusList.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                SetProperty(ref _isRunning, value);     
            }
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
        public float Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value); 
            }
        }
        public bool IsAddEnabled
        {
            get => _isAddEnabled;
            set
            {
                SetProperty(ref _isAddEnabled, value);
            }
        }
        private async void OnAddLineClicked(object obj)
        {
            await Shell.Current.GoToAsync(nameof(RFCensusProductSelectionPage));  
        }
        private async void OnDeletePressed(RFCensus l)
        {
            if (l == null)
                return;
            RFCensusList.Remove(l);
            await RFCensusRepo.DeleteItemAsync(l.Oid.ToString());
        }
    }
}
