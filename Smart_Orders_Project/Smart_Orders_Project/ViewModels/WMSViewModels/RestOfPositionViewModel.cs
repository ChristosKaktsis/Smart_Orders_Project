using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SmartMobileWMS.Models;
using SmartMobileWMS.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SmartMobileWMS.Repositories;
using System.Diagnostics;

namespace SmartMobileWMS.ViewModels
{
    public class RestOfPositionViewModel : BaseViewModel
    {
        public Command EmptyPositionCommand { get; set; }
        public ObservableCollection<Product> ProductList { get; set; }
        public Position Position 
        { 
            get =>position;
            set
            {
                SetProperty(ref position, value);
                PositionHasError = value == null;
            }
        }
        public bool PositionHasError
        {
            get { return positionHasError; }
            set
            {
                SetProperty(ref positionHasError, value);
                if (value)
                    ErrorMessage = "Η θέση δεν βρέθηκε";
            }
        }
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                SetProperty(ref errorMessage, value);

            }
        }
        private Position position;
        private string errorMessage;
        private bool positionHasError;
        private PositionChangeRepository positionChangeRepository = new PositionChangeRepository();
        public RestOfPositionViewModel()
        {
            InitializeModel();
        }

        private void InitializeModel()
        {
            ProductList = new ObservableCollection<Product>();
            EmptyPositionCommand = new Command(async () => await EmptyPosition());
        }
        public async Task LoadPosition(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return;
                IsBusy = true;
                Position = await positionRepository.GetItemAsync(id);
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                PositionHasError = true;
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task LoadProductsFromPosition(Position position)
        {
            try
            {
                if (position == null)
                    return;
                IsBusy = true;
                ProductList.Clear();
                var items = await productRepository.GetItemsAsync(position.Oid.ToString());
                if (items == null) return;
                foreach (var item in items)
                    AddItemToList(item);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void AddItemToList(Product item)
        {
            if (item.Quantity == 0 && !ZeroValues)
                return;
            ProductList.Add(item);
        }

        public async Task EmptyPosition()
        {
            if (Position == null)
                return;
            if (!await Continue()) return;
            try
            {
                IsBusy = true;
                foreach (var item in ProductList)
                {
                    PositionChange positionChange = null;
                    if (item.Quantity > 0)
                    {
                        positionChange = new PositionChange
                        {
                            Position = Position,
                            Product = item,
                            Quantity = item.Quantity,
                            Type = 1,
                        };
                    }
                    else if (item.Quantity < 0)
                    {
                        positionChange = new PositionChange
                        {
                            Position = Position,
                            Product = item,
                            Quantity = item.Quantity * -1,
                            Type = 0,
                        };
                    }
                    var result = await positionChangeRepository.AddItem(positionChange);
                    item.Quantity = 0;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", $"Η ενέργεια δεν πραγματοποιήθηκε", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<bool> Continue() => await Shell.Current.DisplayAlert(
            "Θέλετε να μηδενίσετε τις τιμές των ειδών ?"
            , $"Οι τιμές τω ειδών στη συγκεκριμένη θέση θα μηδενιστούν"
            ,"Μηδενισμός","Άκυρο");
    }
}
