using Smart_Orders_Project.Models;
using Smart_Orders_Project.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class OrdersDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string customerName;
        private string orderOid;
        public ObservableCollection<LineOfOrder> LinesList { get; set; }
        public Command AddLine { get; }
        public Command SelectCustomer { get; }
        public Command LoadItemsCommand { get; }
        public Command SaveRFSalesCommand { get; }
        public OrdersDetailViewModel()
        {
            AddLine = new Command(OnAddLineClicked);
            SelectCustomer = new Command(OnSelectCustomerClicked);
            LinesList = new ObservableCollection<LineOfOrder>();
            OrderOid = Guid.NewGuid().ToString();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SaveRFSalesCommand = new Command(ExecuteSaveRFSalesCommand, ValidateSave);
            this.PropertyChanged +=
                (_, __) => SaveRFSalesCommand.ChangeCanExecute();
        }

        private async void ExecuteSaveRFSalesCommand()
        {  
            try
            {
                RFSales sale1 = new RFSales()
                {
                    Oid = Guid.Parse(OrderOid),
                    Customer = await CustomerRepo.GetItemAsync(itemId),
                    Lines = LinesList.ToList(),
                    CreationDate = DateTime.Now
                };
                await RFSalesRepo.AddItemAsync(sale1);
                await RFSalesRepo.UploadItemAsync(sale1);
                foreach(var i in sale1.Lines)
                {
                    await LinesRepo.UploadItemAsync(i);
                    await LinesRepo.DeleteItemAsync(i.Oid.ToString());
                }
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            
        }
        private bool ValidateSave()
        {
            return !string.IsNullOrEmpty(CustomerName);
        }
        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                LinesList.Clear();
                var items = await LinesRepo.GetItemsAsync(true);
                foreach (var item in items)
                {
                    LinesList.Add(item);
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
                SetProperty(ref orderOid, value);
            }
        }
        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await CustomerRepo.GetItemAsync(itemId);
                CustomerName = string.IsNullOrEmpty(item.AltName) ? item.Name : item.AltName;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
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
