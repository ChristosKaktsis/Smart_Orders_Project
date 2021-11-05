using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    class CustomerSelectionViewModel : BaseViewModel
    {
        private Customer _selectedCustomer;
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
                var items = await CustomerRepo.GetItemsAsync(true);
                foreach (var item in items)
                {
                    CustomerList.Add(item);
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
