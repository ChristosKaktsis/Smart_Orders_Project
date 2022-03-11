using Smart_Orders_Project.Models;
using Smart_Orders_Project.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    [QueryProperty(nameof(ProviderID), nameof(ProviderID))]
    [QueryProperty(nameof(RFPurchaseID), nameof(RFPurchaseID))]
    public class RFPurchaseDetailViewModel : BaseViewModel
    {
        private string providerName = "Επιλογή Προμηθευτή";
        private RFPurchase rfPurchase;
        private string providerDoc;
        private RFPurchaseLine selectedLine;
        private bool isSelected, _isAddEnabled = true, _isWHLEnabled, _isHEnabled, _isLEnabled, _isWEnabled;
        private float _quantity, _height, _unit, _length, _width;
        private string _thisisa;
        private double _sum;

        public ObservableCollection<RFPurchaseLine> LinesList { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command SelectProviderCommand { get; set; }
        public Command SelectProductCommand { get; set; }
        public Command SetLineCommand { get; set; }
        public Command SaveCommand { get; set; }
        public RFPurchaseDetailViewModel()
        {
            InitializeModel();
            SelectProviderCommand = new Command(ExecuteSelectProviderCommand);
            SelectProductCommand = new Command(ExecuteSelectProductCommand);
            SetLineCommand = new Command(SetLineValues);
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SaveCommand = new Command(ExecuteSaveCommand, ValidateSave);
            this.PropertyChanged +=
               (_, __) => SaveCommand.ChangeCanExecute();
        }
        private bool ValidateSave()
        {
            return ProviderName != "Επιλογή Προμηθευτή" && LinesList.Any();
        }
        private async void ExecuteSaveCommand()
        {
            try
            {
                AddLinesToPurchase();
                //check if exist
                var exist = await App.Database.GetPurchaseAsync(rFPurchase.Oid.ToString());
                if (exist == null)
                {
                    await App.Database.AddPurchaseAsync(rFPurchase);
                }
                else
                    //update
                    await RFPurchaseRepo.UpdateItemAsync(rFPurchase);

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            await Shell.Current.GoToAsync("..");
        }

        private void AddLinesToPurchase()
        {
            foreach (var item in LinesList)
                if(!rFPurchase.Lines.Contains(item))
                    rFPurchase.Lines.Add(item);
        }

        private  void InitializeModel()
        {
            //view model lives after going to the next page
            //
            LinesList = new ObservableCollection<RFPurchaseLine>();

        }
        public RFPurchase rFPurchase 
        { 
            get 
            { 
                if(rfPurchase== null)
                {
                    rfPurchase = new RFPurchase
                    {
                        Oid = Guid.NewGuid(),
                        Lines = new List<RFPurchaseLine>(),
                        CreationDate = DateTime.Now,
                    };
                }
                return rfPurchase;
            }
            set
            {
                rfPurchase = value;
                AddLinesToData(value);
                ProviderDoc = value.ProviderDoc;
                ProviderName = value.Provider.Name;
            } 
        }

        private  void AddLinesToData(RFPurchase value)
        {
            if (value == null)
                return;
            foreach (var item in value.Lines)
                if (!LinesList.Contains(item))
                    LinesList.Add(item);
        }

        private async void ExecuteSelectProductCommand(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(Views.RFPurchaseLineSelectionPage)}?{nameof(RFPurchaseLinesViewModel.ItemId)}={rFPurchase.Oid}");
        }

        public string ProviderName
        {
            get
            {
                return providerName;
            }
            set
            {
                SetProperty(ref providerName, value);
            }
        }
        public string ProviderDoc
        {
            get
            {
                return providerDoc;
            }
            set
            {
                SetProperty(ref providerDoc, value);
                if (!string.IsNullOrEmpty(value))
                    rFPurchase.ProviderDoc = value;
            }
        }
        public  void OnAppearing()
        {
            IsBusy = true;
        }
        private async void ExecuteSelectProviderCommand()
        {
            await Shell.Current.GoToAsync(nameof(ProviderSelectionPage));
        }

        public string ProviderID
        {
            set
            {
                LoadProvider(value);
            }
        }
        public string RFPurchaseID
        {
            set
            {
                LoadRFPurchase(value);
            }
        }

        private async void LoadRFPurchase(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;
            try
            {
                rFPurchase = await App.Database.GetPurchaseAsync(value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task ExecuteLoadItemsCommand() 
        {
            IsBusy = true;
            try
            {
                
                var items = await App.Database.GetPurchaseLineAsync();
                foreach(var item in items)
                {
                    if(!LinesList.Contains(item))
                        LinesList.Add(item);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void LoadProvider(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;
            try
            {
                var provider = await ProviderRepo.GetItemAsync(value);
                ProviderName = provider.Name;
                AddProviderToPurchase(provider);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        private void AddProviderToPurchase(Provider provider)
        {
            if (provider == null)
                return;

            rFPurchase.Provider = provider;
        }
        public RFPurchaseLine SelectedLine
        {
            get
            {
                return selectedLine;
            }
            set
            {
                SetProperty(ref selectedLine, value);
                EnableButtons(value);
            }
        }
        private void EnableButtons(RFPurchaseLine value)
        {
            if (value != null)
            {
                Width = (float)value.Width;
                Height = (float)value.Height;
                Length = (float)value.Length;
                Quantity = (float)value.Quantity;
                IsWHLEnabled = true;
                IsAddEnabled = false;
                switch (value.Product.Type)
                {
                    case 0:
                        IsWEnabled = false;
                        IsLEnabled = false;
                        IsHEnabled = false;
                        break;
                    case 1:
                        IsWEnabled = false;
                        IsLEnabled = true;
                        IsHEnabled = false;
                        break;
                    case 2:
                        IsWEnabled = true;
                        IsLEnabled = true;
                        IsHEnabled = false;
                        ThisIsA = "Τετρ.Μέτρα";
                        break;
                    case 3:
                        IsWEnabled = true;
                        IsLEnabled = true;
                        IsHEnabled = true;
                        ThisIsA = "Κυβ.Μέτρα";
                        break;
                }
            }
            else
            {
                IsWEnabled = false;
                IsLEnabled = false;
                IsHEnabled = false;
                IsWHLEnabled = false;
                IsAddEnabled = true;
            }
        }
        public double Sum
        {
            get => _sum;
            set
            {
                SetProperty(ref _sum, value);
            }
        }
        public float Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value);
                CheckSum();
                
            }
        }
        public float Height
        {
            get => _height;
            set
            {
                SetProperty(ref _height, value);
                CheckSum();
                
            }
        }
        public float Width
        {
            get => _width;
            set
            {
                SetProperty(ref _width, value);
                CheckSum();
                
            }
        }
        public float Length
        {
            get => _length;
            set
            {
                SetProperty(ref _length, value);
                CheckSum();
                
            }
        }
        private void CheckSum()
        {
            if (SelectedLine == null)
                return;

            float oldwidth = SelectedLine.Product.Width == 0 ? 1 : SelectedLine.Product.Width;
            float oldlength = SelectedLine.Product.Length == 0 ? 1 : SelectedLine.Product.Length;
            float oldheight = SelectedLine.Product.Height == 0 ? 1 : SelectedLine.Product.Height;
            switch (SelectedLine.Product.Type)
            {
                case 1:
                    oldwidth = 1;
                    oldheight = 1;
                    break;
                case 2:
                    oldheight = 1;
                    break;
            }
            float oldunit = (oldwidth * oldlength) * oldheight;
            float newunit = (Width * Length) * Height;

            if (SelectedLine.Product.Length == 0)
            {
                Sum = Quantity * SelectedLine.Product.Price;
            }
            else
            {
                Unit = newunit * Quantity;
                if (oldunit == 0)
                {
                    Sum = 0;
                }
                else
                {
                    var athr = (newunit * SelectedLine.Product.Price) / oldunit;
                    Sum = Quantity * athr;
                }
            }
        }
        private void SetLineValues()
        {
            if (SelectedLine == null)
                return;
            SelectedLine.Quantity = (decimal)Quantity;
            SelectedLine.Width = (decimal)Width;
            SelectedLine.Height = (decimal)Height;
            SelectedLine.Length = (decimal)Length;
            SelectedLine.Sum = Sum;
            SelectedLine = null;
        }
        public string ThisIsA
        {
            get => _thisisa;
            set
            {
                SetProperty(ref _thisisa, value);
            }
        }
        public float Unit
        {
            get => _unit;
            set
            {
                SetProperty(ref _unit, value);
            }
        }
        public bool IsWEnabled
        {
            get => _isWEnabled;
            set
            {
                SetProperty(ref _isWEnabled, value);
            }
        }
        public bool IsLEnabled
        {
            get => _isLEnabled;
            set
            {
                SetProperty(ref _isLEnabled, value);
            }
        }
        public bool IsHEnabled
        {
            get => _isHEnabled;
            set
            {
                SetProperty(ref _isHEnabled, value);
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
        public bool IsAddEnabled
        {
            get => _isAddEnabled;
            set
            {
                SetProperty(ref _isAddEnabled, value);
            }
        }
    }
}
