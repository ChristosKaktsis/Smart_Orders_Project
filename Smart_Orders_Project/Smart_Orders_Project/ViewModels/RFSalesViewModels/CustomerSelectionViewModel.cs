using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    class CustomerSelectionViewModel : BaseViewModel
    {
        private Customer _selectedCustomer;
        private string _search;

        public ObservableCollection<Customer> CustomerList { get; }
        public Command LoadItemsCommand { get; }
        public Command AddCustomerCommand { get; }
        public CustomerSelectionViewModel()
        {
            CustomerList = new ObservableCollection<Customer>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddCustomerCommand = new Command(AddCustomerClicked);
        }

        private async void AddCustomerClicked(object obj)
        {
            if (SelectedCustomer == null)
                return;
            await Shell.Current.GoToAsync($"..?{nameof(OrdersDetailViewModel.ItemId)}={SelectedCustomer.Oid}");
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                CustomerList.Clear();
                //var items = await CustomerRepo.GetItemsAsync(true);
                var items = await CustomerRepo.GetItemsWithNameAsync(Search);
                foreach (var item in items)
                {
                    CustomerList.Add(item);
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
        public void OnAppearing()
        {
            IsBusy = false;
            
        }
        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                LoadItemsCommand.Execute(null);
            }
        }
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                SetProperty(ref _selectedCustomer, value);       
            }
        }
    }
}
