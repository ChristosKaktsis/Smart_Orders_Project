using SmartMobileWMS.Models;
using SmartMobileWMS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    public class SparePartViewModel : BaseViewModel
    {
        private bool _isFocused;
        private string _searchText;
        public ObservableCollection<Product> ProductList { get; }
        public Command LoadItemsCommand { get; }
        public Command AddSparePart { get; }
        public SparePartViewModel()
        {
            AddSparePart = new Command(AddSparePartClicked);
            ProductList = new ObservableCollection<Product>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }
        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                ProductList.Clear();
                
                if (SearchText.Length == 13)
                {
                    var it = await productRepository.GetItemAsync(SearchText);
                    if (it != null)
                        ProductList.Add(it);
                    else
                        await Shell.Current.DisplayAlert("Barcode!", "το είδος δεν βρέθηκε", "Οκ");
                }
                //else
                //{
                //    var items = await ProductRepo.GetItemsWithNameAsync(SearchText);

                //    foreach (var item in items)
                //    {
                //        ProductList.Add(item);
                //    }
                //}
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
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
            }
        }
        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                SetProperty(ref _isFocused, value);
                if (!value)
                {
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        //να μην εκτελεστει γιατι γινεται φιλτραρισμα στο UI 
                        //οταν εχει δυο και παραπανω λεξεις αναλαμβανει το UI
                        string[] subs = SearchText.Split(' ');
                        if (subs.Length > 1)
                            return;
                        //
                        LoadItemsCommand.Execute(null);
                    }
                }

            }
        }
        private async void AddSparePartClicked(object obj)
        {
            await Shell.Current.GoToAsync(nameof(TreeGroupingPage));
        }
    }
}
