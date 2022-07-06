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
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class LineOfOrdersViewModel : BaseLineViewModel
    {

        private string itemId;

        public LineOfOrdersViewModel()
        {
            ProductList = new ObservableCollection<Product>();
            //SelectedProductList = new ObservableCollection<Product>();
            //SelectedProductList.CollectionChanged += SelectedProductList_CollectionChanged;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            //LoadItemCommand = new Command(async () => await ExecuteLoadItemCommand());
            SaveCommand = new Command(OnSave, ValidateSave);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            ScannerCommand = new Command(OnScannerClicked);
        }
        private async void OnSave()
        {
            LineOfOrder newItem = new LineOfOrder()
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

            await LinesRepo.AddItemAsync(newItem);

            await Shell.Current.GoToAsync("..");
            if (IsQuickOn)
            {
                await Shell.Current.GoToAsync($"{nameof(Views.LineOfOrdersSelectionPage)}?{nameof(LineOfOrdersViewModel.ItemId)}={ItemId}");
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
