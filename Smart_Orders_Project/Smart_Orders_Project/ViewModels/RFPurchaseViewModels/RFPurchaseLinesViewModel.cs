using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
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
        protected override void CheckSum()
        {
            if (SelectedProduct == null)
                return;

            float oldwidth = SelectedProduct.Width == 0 ? 1 : SelectedProduct.Width;
            float oldlength = SelectedProduct.Length == 0 ? 1 : SelectedProduct.Length;
            float oldheight = SelectedProduct.Height == 0 ? 1 : SelectedProduct.Height;

            switch (SelectedProduct.Type)
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

            if (SelectedProduct.Length == 0)
            {
                Sum = Quantity * SelectedProduct.LastPriceSold;
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
                    var athr = (newunit * SelectedProduct.LastPriceSold) / oldunit;
                    Sum = Quantity * athr;
                }
            }

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
