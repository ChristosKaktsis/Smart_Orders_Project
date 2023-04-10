using SmartMobileWMS.Models;
using SmartMobileWMS.Models.SparePartModels;
using SmartMobileWMS.Repositories;
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
        public IDataStore<Brand> BrandRepo => DependencyService.Get<IDataStore<Brand>>();
        public IDataStore<Manufacturer> ManufacturerRepo => DependencyService.Get<IDataStore<Manufacturer>>();
        public IDataGet<Grouping> GroupingRepo => DependencyService.Get<IDataGet<Grouping>>();
        //
        public RepositoryModel ModelRepo = new RepositoryModel();
        public RepositorySparePart SparePartRepo = new RepositorySparePart();
        //new 
        protected ProductRepository productRepository = new ProductRepository();
        protected PositionRepository positionRepository = new PositionRepository();
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
        public int SSCCDigits
        {
            get => Preferences.Get(nameof(SSCCDigits), 0);
            set
            {
                Preferences.Set(nameof(SSCCDigits), value);
            }
        }
        public int SSCCStart
        {
            get => Preferences.Get(nameof(SSCCStart), 0);
            set
            {
                Preferences.Set(nameof(SSCCStart), value);
            }
        }
        public int SSCCEnd
        {
            get => Preferences.Get(nameof(SSCCEnd), 0);
            set
            {
                Preferences.Set(nameof(SSCCEnd), value);
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
        protected async void NotifySNNotValid()
        {
            await Shell.Current.DisplayAlert("Το Serial Number υπάρχει",
                "Δεν μπορείτε να εισάγεται στη λίστα το ίδιο Serial Number πάνω απο μία φορές", 
                "Οκ");
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
