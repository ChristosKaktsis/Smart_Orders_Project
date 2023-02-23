using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.ViewModels
{
    public class RestOfProductsViewModel : PositionBaseViewModel
    {
        public ObservableCollection<Position> PositionList { get; set; }
        public RestOfProductsViewModel()
        {
            Initializemodel();
        }

        private void Initializemodel()
        {
            PositionList = new ObservableCollection<Position>();
        }
        public async Task LoadPositions()
        {
            try
            {
                if (Product == null)
                    return;
                IsBusy = true;
                PositionList.Clear();
                var items = await positionRepository.GetItemsAsync(Product.CodeDisplay);
                foreach (var item in items)
                    AddItemToList(item);
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
        private void AddItemToList(Position item)
        {
            if (item.ItemQuantity == 0 && !ZeroValues)
                return;
            PositionList.Add(item);
        }
    }
}
