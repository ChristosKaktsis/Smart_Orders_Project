using Smart_Orders_Project.Models;
using Smart_Orders_Project.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    public class PositionImportExportViewModel : PositionBaseViewModel
    {
        protected int im_ex;
        
        public async Task ExecuteSavePosition(int type)
        {
            try
            {
                var result = await positionChange.PositionChange(Position, Product, Quantity, type);
                Console.WriteLine($"Saved? {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Shell.Current.DisplayAlert("Προσοχή!", $"Κάτι πήγε στραβά :{ex}", "Ok");
            }

        }
       
       

    }
}
