using SmartMobileWMS.Models;
using SmartMobileWMS.Views;
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
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    [QueryProperty(nameof(OrderOid), nameof(OrderOid))]
    class OrdersDetailViewModel : BaseViewModel
    {
        private string itemId;  
        private string customerName="Επιλογή Πελάτη";
        private string orderOid;
        private RFSales rfsale;
        private double _sum;
        private float _length;
        private float _width;
        private float _height;
        private float _quantity;
        private LineOfOrder _line;
        private bool _isWEnabled;
        private bool _isLEnabled;
        private bool _isWHLEnabled;
        private bool _isHEnabled;
        private bool _isAddEnabled = true;
        private Reciever _reciever;
        private float _unit;
        private string _thisisa;

        public ObservableCollection<LineOfOrder> LinesList { get; set; }
        public ObservableCollection<Reciever> RecieverList { get; set; }
        public Command AddLine { get; }
        public Command SelectCustomer { get; }
        public Command LoadItemsCommand { get; }
        public Command SaveRFSalesCommand { get; }
        public Command SaveCommand { get; }
        public Command BackCommand { get; }
        public Command DeleteCommand { get; }
        public OrdersDetailViewModel()
        {
            Title = "RF"+RFCounter.ToString().PadLeft(8, '0');
            AddLine = new Command(OnAddLineClicked);
            SelectCustomer = new Command(OnSelectCustomerClicked);
            LinesList = new ObservableCollection<LineOfOrder>();
            RecieverList = new ObservableCollection<Reciever>();
            OrderOid = Guid.NewGuid().ToString(); // παντα το βάζει αμα ειναι καινουρια πώληση αλλα αμα ερθεις απο edit εκτελείτε μετα και το Oid που ερχεται
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SaveRFSalesCommand = new Command(ExecuteSaveRFSalesCommand, ValidateSave);
            this.PropertyChanged +=
                (_, __) => SaveRFSalesCommand.ChangeCanExecute();
            BackCommand = new Command(OnBackButtonPressed);
            DeleteCommand = new Command<LineOfOrder>(OnDeletePressed);
            SaveCommand = new Command(OnLineSave);
        }

        private void OnLineSave(object obj)
        {    
            SelectedLine.Quantity = (decimal)Quantity;
            SelectedLine.Width = (decimal)Width;
            SelectedLine.Height = (decimal)Height;
            SelectedLine.Length = (decimal)Length;
            SelectedLine.Sum = Sum;
            SelectedLine = null;
        }
        private async void OnDeletePressed(LineOfOrder l)
        {
            if (l == null)
                return;
            LinesList.Remove(l);
            await LinesRepo.DeleteItemAsync(l.Oid.ToString());
            if (RfSale != null)
                RfSale.Lines.Remove(l);
        }
        private async void OnBackButtonPressed(object obj)
        {
            var answer = await Shell.Current.DisplayAlert("Ερώτηση;", "Θέλετε να αποχορήσετε", "Ναί", "Όχι");
            if (answer)
            {
                //remove the items from the cart before going back
                foreach (var i in LinesList)
                {
                    await LinesRepo.DeleteItemAsync(i.Oid.ToString());
                }
                // This will pop the current page off the navigation stack
                GoBack();
            }      
        }
        private async void ExecuteSaveRFSalesCommand()
        {  
            try
            {
                if (RfSale == null)//if is null means is not from edit 
                {
                    RFSales sale1 = new RFSales()
                    {
                        Oid = Guid.Parse(OrderOid),
                        Customer = await CustomerRepo.GetItemAsync(itemId),
                        Lines = LinesList.ToList(),
                        RFCount =Title,
                        CreationDate = DateTime.Now,
                        Reciever = Reciever 
                    };
                    RFCounter++;
                    await RFSalesRepo.AddItemAsync(sale1);
                    await RFSalesRepo.UploadItemAsync(sale1);
                    foreach (var i in sale1.Lines)
                    {
                        await LinesRepo.UploadItemAsync(i);
                        await LinesRepo.DeleteItemAsync(i.Oid.ToString());
                    }
                    //RFCounter++;
                }
                else
                {
                    RfSale.Customer = await CustomerRepo.GetItemAsync(itemId);
                    RfSale.Reciever = Reciever;
                    await RFSalesRepo.UpdateItemAsync(RfSale);
                    RfSale.Lines = LinesList.ToList();
                    var items = RfSale.Lines;
                    foreach (var i in items)
                    {
                        await LinesRepo.UploadItemAsync(i);
                        await LinesRepo.DeleteItemAsync(i.Oid.ToString());
                    }
                }
                
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Σφάλμα!", "ExecuteSaveRFSalesCommand \n" + ex.Message, "Οκ");
            }
            
        }
        private bool ValidateSave()
        {
            return CustomerName != "Επιλογή Πελάτη" && LinesList.Any();
        }
        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                LinesList.Clear();
                RecieverList.Clear();
                if (RfSale != null)
                {
                    foreach (var line in RfSale.Lines)
                    {
                        line.Sum = CheckSumInLoadItems(line);
                        LinesList.Add(line);
                        Reciever = RfSale.Reciever;
                    }
                }
                var items = await LinesRepo.GetItemsAsync(true);
                foreach (var item in items)
                {
                    LinesList.Add(item);
                }
                //reciever list add
                var recieveritems = await RecieverRepo.GetItemsAsync(true);
                foreach (var item in recieveritems)
                {
                    RecieverList.Add(item);
                }
                RecieverList.Add(new Reciever { RecieverName="Νέος Παραλαβών"});
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

        private double CheckSumInLoadItems(LineOfOrder line)
        {
            if (line == null)
                return 0;

            float oldwidth = line.Product.Width == 0 ? 1 : line.Product.Width;
            float oldlength = line.Product.Length == 0 ? 1 : line.Product.Length;
            float oldheight = line.Product.Height == 0 ? 1 : line.Product.Height;
            double sumin = 0;
            switch (line.Product.Type)
            {
                case 1:
                    oldwidth = 1;
                    oldheight = 1;
                    line.Width = 1;
                    line.Height = 1;
                    break;
                case 2:
                    oldheight = 1;
                    line.Height = 1;
                    break;
            }
            float oldunit = (oldwidth * oldlength) * oldheight;
            float newunit = (float)((line.Width * line.Length) * line.Height);

            if (line.Product.Length == 0)
            {
                sumin = (double)line.Quantity * line.Product.Price;
            }
            else
            {
                if (oldunit == 0)
                {
                    sumin = 0;
                }
                else
                {
                    var athr = (newunit * line.Product.Price) / oldunit;
                    sumin = (double)line.Quantity * athr;
                }
            }
            return sumin;
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
        public string CustomerName
        {
            get => customerName;
            set => SetProperty(ref customerName, value);
        }
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }
        public string OrderOid
        {
            get
            {
                return orderOid;
            }
            set
            {
                if (value == null)
                    return;
                SetProperty(ref orderOid, value);
                GetOrder(value);
            }
        }
        public RFSales RfSale
        {
            get
            {
                return rfsale;
            }
            set
            {
                SetProperty(ref rfsale, value);
                if(value!=null)
                    Title = value.RFCount;
            }
        }
        public Reciever Reciever
        {
            get
            {
                return _reciever;
            }
            set
            {
                SetProperty(ref _reciever, value);
                if (value == null)
                    return;
                if (value.RecieverName == "Νέος Παραλαβών")
                {
                    GoToNewReciever();
                }
            }
        }

        private async void GoToNewReciever()
        {
            await Shell.Current.GoToAsync(nameof(NewRecieverPage));
        }
        private async void GetOrder(string value)
        {
            try
            {
                RfSale = await RFSalesRepo.GetItemAsync(value);
                if (RfSale != null)
                {
                    ItemId = rfsale.Customer.Oid.ToString();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Σφάλμα!", "GetOrder \n" + ex.Message, "Οκ");
            }
            
               
        }
        public LineOfOrder SelectedLine
        {
            get => _line;
            set
            {
                SetProperty(ref _line, value);
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
        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await CustomerRepo.GetItemAsync(itemId);
                CustomerName = string.IsNullOrEmpty(item.AltName) ? item.Name : item.AltName;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to Load Item");
                await Shell.Current.DisplayAlert("Σφάλμα!", "LoadItemId \n" + ex.Message, "Οκ");
            }
        }
        private async void OnAddLineClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync(nameof(LineOfOrdersSelectionPage));
            await Shell.Current.GoToAsync($"{nameof(LineOfOrdersSelectionPage)}?{nameof(LineOfOrdersViewModel.ItemId)}={OrderOid}");
        }

        private async void OnSelectCustomerClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync(nameof(CustomerSelectionPage));
        }
    }
}
