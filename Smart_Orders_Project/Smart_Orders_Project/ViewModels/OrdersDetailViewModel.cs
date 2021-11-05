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
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class OrdersDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string customerName;
        public ObservableCollection<LineOfOrder> LinesList { get; set; }
        public Command AddLine { get; }
        public Command SelectCustomer { get; }
        public Command LoadItemsCommand { get; }
        public OrdersDetailViewModel()
        {
            AddLine = new Command(OnAddLineClicked);
            SelectCustomer = new Command(OnSelectCustomerClicked);
            LinesList = new ObservableCollection<LineOfOrder>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
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
            await Shell.Current.GoToAsync(nameof(LineOfOrdersSelectionPage));
        }
        private async void OnSelectCustomerClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync(nameof(CustomerSelectionPage));
        }
    }
}
