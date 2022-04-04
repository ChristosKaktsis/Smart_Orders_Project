using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    public class MoveToPositionViewModel : PositionBaseViewModel
    {
        private Position positionTo;
        private string positionToid;
        private bool positionToHasError;
        private bool deletebuttonisvisible = false;

        public ObservableCollection<Product> ProductList { get; set; }
        public ObservableCollection<Product> SelectedProductList { get; set; }
        public Command RemoveItems { get; set; }
        public MoveToPositionViewModel()
        {
            ProductList = new ObservableCollection<Product>();
            SelectedProductList = new ObservableCollection<Product>();
            SelectedProductList.CollectionChanged += SelectedProductList_CollectionChanged;
            RemoveItems = new Command(ExecuteRemoveItems);
        }

        private void SelectedProductList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                DeleteButtonIsVisible = true;
            else if (!SelectedProductList.Any())
                 DeleteButtonIsVisible = false;
        }

        public bool DeleteButtonIsVisible
        {
            get { return deletebuttonisvisible; }
            set
            {
                SetProperty(ref deletebuttonisvisible, value);
            }
        }
        private void ExecuteRemoveItems()
        {
            foreach (var item in SelectedProductList)
                ProductList.Remove(item);

            SelectedProductList.Clear();
        }

        public string PositionToID
        {
            get { return positionToid; }
            set
            {
                positionToid=value;
            }
        }
        
        public Position PositionTo
        {
            get { return positionTo; }
            set
            {
                SetProperty(ref positionTo, value);
            }
        }
        public async Task SetPositionTo(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            IsBusy = true;
            try
            {
                PositionTo = await RFPositionRepo.GetItemAsync(value);
                PositionHasError = PositionTo == null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public bool PositionToHasError
        {
            get { return positionToHasError; }
            set
            {
                SetProperty(ref positionToHasError, value);
                if (value)
                    ErrorMessage = "Η θέση δεν βρέθηκε";
            }
        }
        public void AddProductToList(Product item)
        {
            if (item == null)
                return;
            item.Quantity = Quantity;
            if (ProductList.All(x => x.Oid != item.Oid))
                ProductList.Add(item);
        }
        public async Task MoveToPosition()
        {
            try
            {
                IsBusy = true;
                if (!Positions_Are_Set())
                    return;
                foreach (var item in ProductList)
                    await positionChange.PositionChange(Position, item, item.Quantity, 1,null);
                //move to positionTo
                foreach (var item in ProductList)
                    await positionChange.PositionChange(PositionTo, item, item.Quantity, 0,null);

                ProductList.Clear();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                await AppShell.Current.DisplayAlert("Σφάλμα", $"MoveToPosition : {ex}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool Positions_Are_Set()
        {
            return Position != null && PositionTo != null;
        }
    }
}
