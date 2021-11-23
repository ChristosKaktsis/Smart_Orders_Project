using Smart_Orders_Project.Models;
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
            get => Preferences.Get(nameof(ConnectionString), @"User Id=sa;password=1;Pooling=false;Data Source=192.168.3.44\SQLEXPRESS;Initial Catalog=maindemo");
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
