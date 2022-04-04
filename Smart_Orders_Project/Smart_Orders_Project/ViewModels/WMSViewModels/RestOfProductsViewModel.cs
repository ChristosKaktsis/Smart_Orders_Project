using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.ViewModels
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
                var items = await positionChange.GetPositionsFromProduct(Product.CodeDisplay);
                foreach (var item in items)
                    PositionList.Add(item);
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
