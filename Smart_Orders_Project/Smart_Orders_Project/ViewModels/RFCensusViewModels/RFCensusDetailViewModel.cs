using SmartMobileWMS.Models;
using SmartMobileWMS.Views;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    class RFCensusDetailViewModel : BaseViewModel
    {
        private float _quantity;
        private bool _isAddEnabled = true;
        private string _searchText;
        private Position _selectedPosition;
        private bool _isFocused;
        private bool _isRunning;
        private bool _isWHLEnabled;
        private RFCensus _selectedRFCensus;
        private Storage _selectedStorage;
        private ImageSource _doneImageSource ;
        private Color _searchTextColor ;
        private string censusOid;
        private RFCensus rfcensus;

        public ObservableCollection<Storage> StorageList { get; set; }
        public ObservableCollection<RFCensus> RFCensusList { get; set; }
        public Command LoadItemsCommand { get; }
        public Command LoadRFPositionCommand { get; }
        public Command AddLine { get; }
        public Command SaveUpdatedRFCensusCommand { get; }
        public Command DeleteCommand { get; }
        public Command BackCommand { get; }
        public RFCensusDetailViewModel()
        {
            StorageList = new ObservableCollection<Storage>();
            RFCensusList = new ObservableCollection<RFCensus>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            LoadRFPositionCommand = new Command(async () => await ExecuteLoadRFPositionCommand());
            AddLine = new Command(OnAddLineClicked,ValidateAddButton);
            this.PropertyChanged +=
               (_, __) => AddLine.ChangeCanExecute();
            DeleteCommand = new Command<RFCensus>(OnDeletePressed);
            SaveUpdatedRFCensusCommand = new Command(OnUpdateRFCensus);
            BackCommand = new Command(OnBackButtonPressed);
        }
        private async void OnUpdateRFCensus(object obj)
        {
            if (SelectedRFCensus == null)
                return;
            SelectedRFCensus.Quantity = (decimal)Quantity;
            await RFCensusRepo.UpdateItemAsync(SelectedRFCensus);
            SelectedRFCensus = null;
        }
        private bool ValidateAddButton(object arg)
        {
            return SelectedPosition != null && SelectedStorage != null;
        }
        
        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                SetProperty(ref _selectedPosition, value);
            }
        }
        public Storage SelectedStorage 
        {
            get => _selectedStorage;
            set
            {
                SetProperty(ref _selectedStorage, value);
                SetStorageToRepository(value);   
            } 
        }
        private async void SetStorageToRepository(Storage value)
        {
            if (value != null)
            {
                StorageID = value.Oid.ToString();
                await RFStorageRepo.AddItemAsync(value);
            }
        }
        public RFCensus SelectedRFCensus
        {
            get => _selectedRFCensus;
            set
            {
                SetProperty(ref _selectedRFCensus, value);
                if(value == null)
                {
                    IsWHLEnabled = false;
                    IsAddEnabled = true;
                }
                else
                {
                    IsWHLEnabled = true;
                    IsAddEnabled = false;
                }
            }
        }
        private async Task ExecuteLoadRFPositionCommand()
        {
            try
            {
                IsRunning = true;
                SelectedPosition = await RFPositionRepo.GetItemAsync(SearchPositionText);
                if (SelectedPosition == null)
                {
                    DoneImageSource = "error5.png";
                    SearchTextColor = Color.Red;
                    await Shell.Current.DisplayAlert("Προσοχή !", "Η θέση δεν βρέθηκε", "ΟΚ");
                }  
                else
                {
                    DoneImageSource = "ok.png";
                    SearchTextColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Σφάλμα!", "ExecuteLoadRFPositionCommand \n" + ex.Message, "Οκ");
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
                //change color back to black
                SearchTextColor = Color.Black;
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
                //first list
                SelectedStorage = await RFStorageRepo.GetItemAsync(StorageID);
                StorageList.Clear();
                var items = await RFStorageRepo.GetItemsAsync(true);
                foreach (var item in items)
                {
                    StorageList.Add(item);
                }
                if(!string.IsNullOrWhiteSpace(StorageID))
                    SelectedStorage = StorageList.Single(x => x.Oid.ToString() == StorageID);

                //second list
                var items2 = await RFCensusRepo.GetItemsAsync(true);
                var user = await UserRepo.GetUser();
               
                foreach (var item in items2)
                {
                    if(!RFCensusList.Contains(item))
                    {
                        RFCensusList.Add(item);
                        AddRFCensusToDB(item);
                    }    
                }               
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Σφάλμα!", "ExecuteLoadItemsCommand \n" + ex.Message, "Οκ");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async void AddRFCensusToDB(RFCensus item)
        {
            try
            {
                item.Storage = SelectedStorage;
                item.Position = SelectedPosition;
                item.UserCreator = await UserRepo.GetUser();
                await RFCensusRepo.UploadItemAsync(item);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
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
        public RFCensus RFCensus
        {
            get
            {
                return rfcensus;
            }
            set
            {
                SetProperty(ref rfcensus, value);
            }
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
        public bool IsWHLEnabled
        {
            get => _isWHLEnabled;
            set
            {
                SetProperty(ref _isWHLEnabled, value);
            }
        }
        public ImageSource DoneImageSource 
        {
            get => _doneImageSource;
            set
            {
                SetProperty(ref _doneImageSource, value);
            } 
        }
        public Color SearchTextColor
        {
            get => _searchTextColor;
            set
            {
                SetProperty(ref _searchTextColor, value);
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
            await RFCensusRepo.DeleteItemFromDBAsync(l.Oid.ToString());
        }
        private async void OnBackButtonPressed(object obj)
        {
            var answer = await Shell.Current.DisplayAlert("Ερώτηση;", "Θέλετε να αποχωρήσετε", "Ναί", "Όχι");
            if (answer)
            {
                //remove the items from the cart before going back
                foreach (var i in RFCensusList)
                {
                    await RFCensusRepo.DeleteItemAsync(i.Oid.ToString());
                }
                // This will pop the current page off the navigation stack
                GoBack();
            }
        }
    }
}
