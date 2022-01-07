using Smart_Orders_Project.Models;
using Smart_Orders_Project.Models.SparePartModels;
using Smart_Orders_Project.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
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
            get => Preferences.Get(nameof(ConnectionString), @"User ID=sa;Password=1;Pooling=false;Data Source=192.168.1.187\SQLEXPRESS2019;Initial Catalog=SmartLobSidall");
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
        public async void GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
        private async void OnBackButtonPressed()
        {
            var answer = await Shell.Current.DisplayAlert("Ερώτηση;", "Θέλετε να αποχορήσετε", "Ναί", "Όχι");
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
