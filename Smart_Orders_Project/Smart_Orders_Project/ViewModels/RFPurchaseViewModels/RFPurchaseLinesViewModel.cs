using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class RFPurchaseLinesViewModel : BaseLineViewModel
    {
        private string itemId;

        public RFPurchaseLinesViewModel()
        {
            ProductList = new ObservableCollection<Product>();
            
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            
            SaveCommand = new Command(OnSave, ValidateSave);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            ScannerCommand = new Command(OnScannerClicked);
        }
        private async void OnSave()
        {
            RFPurchaseLine newItem = new RFPurchaseLine()
            {
                Oid = Guid.NewGuid(),
                Product = SelectedProduct,
                Quantity = (decimal)Quantity,
                Width = (decimal)Width,
                Height = (decimal)Height,
                Length = (decimal)Length,
                Sum = Sum,
                RFSalesOid = Guid.Parse(ItemId)
            };

            await App.Database.AddPurchaseLineAsync(newItem);

            await Shell.Current.GoToAsync("..");
            if (IsQuickOn)
            {
                await Shell.Current.GoToAsync($"{nameof(Views.RFPurchaseLineSelectionPage)}?{nameof(RFPurchaseLinesViewModel.ItemId)}={ItemId}");
            }

            // This will pop the current page off the navigation stack
            //await Shell.Current.GoToAsync("..");
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
            }
        }
    }
}
