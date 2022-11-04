using SmartMobileWMS.Models;
using SmartMobileWMS.Models.SparePartModels;
using SmartMobileWMS.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Customer> CustomerRepo => DependencyService.Get<IDataStore<Customer>>();
        public IDataStore<Product> ProductRepo => DependencyService.Get<IDataStore<Product>>();
        public IDataStore<LineOfOrder> LinesRepo => DependencyService.Get<IDataStore<LineOfOrder>>();
        public IDataStore<RFSales> RFSalesRepo => DependencyService.Get<IDataStore<RFSales>>();
        public IDataStore<Storage> RFStorageRepo => DependencyService.Get<IDataStore<Storage>>();
        public IDataStore<Position> RFPositionRepo => DependencyService.Get<IDataStore<Position>>();
        public IDataStore<RFCensus> RFCensusRepo => DependencyService.Get<IDataStore<RFCensus>>();
        public IUser<User> UserRepo => DependencyService.Get<IUser<User>>();
        public IDataStore<Reciever> RecieverRepo => DependencyService.Get<IDataStore<Reciever>>();
        public IDataStore<Brand> BrandRepo => DependencyService.Get<IDataStore<Brand>>();
        public IDataStore<Manufacturer> ManufacturerRepo => DependencyService.Get<IDataStore<Manufacturer>>();
        public IDataGet<Grouping> GroupingRepo => DependencyService.Get<IDataGet<Grouping>>();
        //
        public RepositoryModel ModelRepo = new RepositoryModel();
        public RepositorySparePart SparePartRepo = new RepositorySparePart();
        public RepositoryRFPurchase RFPurchaseRepo = new RepositoryRFPurchase();
        public RepositoryProvider ProviderRepo = new RepositoryProvider();
        public RepositoryRFPurchaseLine RFPurchaseLineRepo = new RepositoryRFPurchaseLine();
        
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
       
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        public bool IsQuickOn
        {
            get => Preferences.Get(nameof(IsQuickOn), false);
            set 
            {
                Preferences.Set(nameof(IsQuickOn), value);
                OnPropertyChanged(nameof(IsQuickOn));
            }
        }
        public string ConnectionString
        {
            get => Preferences.Get(nameof(ConnectionString), App.Current.Resources["ConnectionString"] as string);
            set
            {
                Preferences.Set(nameof(ConnectionString), value);
                OnPropertyChanged(nameof(ConnectionString));
            }
        }
        public string BarCode
        {
            get => Preferences.Get(nameof(BarCode),"");
            set
            {
                Preferences.Set(nameof(BarCode), value);
                OnPropertyChanged(nameof(BarCode));
                GoBack();
            }
        }
        public int RFCounter
        {
            get => Preferences.Get(nameof(RFCounter), 1);
            set
            {
                Preferences.Set(nameof(RFCounter), value);
                OnPropertyChanged(nameof(RFCounter));
            }
        }
        public string UserString
        {
            get => Preferences.Get(nameof(UserString), "");
            set
            {
                Preferences.Set(nameof(UserString), value);
                OnPropertyChanged(nameof(UserString));
            }
        }
        public string SearchPositionText
        {
            get => Preferences.Get(nameof(SearchPositionText), "");
            set
            {
                Preferences.Set(nameof(SearchPositionText), value);
                OnPropertyChanged(nameof(SearchPositionText));

            }
        }
        public string StorageID
        {
            get => Preferences.Get(nameof(StorageID), "");
            set
            {
                Preferences.Set(nameof(StorageID), value);
                OnPropertyChanged(nameof(StorageID));

            }
        }
        public int GTIN
        {
            get => Preferences.Get(nameof(GTIN), 52);
            set
            {
                Preferences.Set(nameof(GTIN), value);
                
            }
        }
        public int Produser
        {
            get => Preferences.Get(nameof(Produser), 99999);
            set
            {
                Preferences.Set(nameof(Produser), value);
                
            }
        }
        public int ArticleFrom
        {
            get => Preferences.Get(nameof(ArticleFrom), 00000);
            set
            {
                Preferences.Set(nameof(ArticleFrom), value);
                
            }
        }
        public int ArticleTo
        {
            get => Preferences.Get(nameof(ArticleTo), 99999);
            set
            {
                Preferences.Set(nameof(ArticleTo), value);
                
            }
        }
        public string SSCC
        {
            get => Preferences.Get(nameof(SSCC), "111");
            set
            {
                Preferences.Set(nameof(SSCC), value);
            }
        }
        public bool ZeroValues //for ypol theshs and thesis eidous
        {
            get => Preferences.Get(nameof(ZeroValues), false);
            set
            {
                Preferences.Set(nameof(ZeroValues), value);
            }
        }
        public async void GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
        private async void OnBackButtonPressed()
        {
            var answer = await Shell.Current.DisplayAlert("Ερώτηση;", "Θέλετε να αποχωρήσετε", "Ναί", "Όχι");
            if (answer)
                GoBack();
        }
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
