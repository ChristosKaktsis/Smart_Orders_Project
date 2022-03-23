using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Smart_Orders_Project.Models;
using Smart_Orders_Project.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Smart_Orders_Project.ViewModels
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

        private RepositoryPositionChange repositoryPosition;
        private Position position;
        private string errorMessage;
        private bool positionHasError;

        public RestOfPositionViewModel()
        {
            InitializeModel();
        }

        private void InitializeModel()
        {
            ProductList = new ObservableCollection<Product>();
            repositoryPosition = new RepositoryPositionChange();
            EmptyPositionCommand = new Command(async () => await EmptyPosition());
        }
        public async Task LoadPosition(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return;
                IsBusy = true;
                Position = await RFPositionRepo.GetItemAsync(id);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
                var items = await repositoryPosition.GetProductsFromList(position.Oid.ToString());
                foreach (var item in items)
                    ProductList.Add(item);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task EmptyPosition()
        {
            if (Position == null)
                return;
            try
            {
                IsBusy = true;
                foreach (var item in ProductList)
                {
                    if (item.Quantity > 0)
                        await repositoryPosition.PositionChange(Position, item, item.Quantity, 1);
                    else if (item.Quantity < 0)
                        await repositoryPosition.PositionChange(Position, item, item.Quantity * -1, 0);
                    item.Quantity = 0;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
