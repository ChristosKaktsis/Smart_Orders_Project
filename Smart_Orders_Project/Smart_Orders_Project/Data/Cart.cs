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
    }
}
