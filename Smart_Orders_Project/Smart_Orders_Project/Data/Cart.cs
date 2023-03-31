using SmartMobileWMS.Models;
using SmartMobileWMS.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.Data
{
    public class Cart
    {/// <summary>
    /// Cart has position change items to be ready to make position export/inport changes
    /// </summary>
        public static ObservableCollection<PositionChange> CartItems 
        { get; } = new ObservableCollection<PositionChange>();
        public static async Task SavePosition(int type)
        {
            try
            {
                var positionChangeRepository = new PositionChangeRepository();
                foreach (var item in CartItems)
                {
                    item.Type = type;
                    if (!await CanExecute(item)) return;
                    var result = await positionChangeRepository.AddItem(item);
                    Debug.WriteLine($"Saved? {result}");
                }
                //empty cart 
                CartItems.Clear();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("", $"Η ενέργεια SavePosition δεν πραγματοποιήθηκε", "Ok");
            }
        }

        private static async Task<bool> CanExecute(PositionChange item)
        {
            if (!item.Product.SN) return true;
            var doesExist = await ProductExistInPosition(
                item.Product.CodeDisplay, item.Position.Oid.ToString());
            if (item.Type == 0)
                return !doesExist;
            return doesExist;
        }
        private static async Task<bool> ProductExistInPosition(string position, string product)
        {
            var productRepository = new ProductRepository();
            var item = await productRepository.GetItemAsync(product, position);
            if (item == null)
                return false;
            return (item.Quantity > 0);
        }
    }
}
