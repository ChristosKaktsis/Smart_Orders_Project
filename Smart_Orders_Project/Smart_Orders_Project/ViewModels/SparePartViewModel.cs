using Smart_Orders_Project.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    public class SparePartViewModel : BaseViewModel
    {
        public Command AddSparePart { get; }
        public SparePartViewModel()
        {
            AddSparePart = new Command(AddSparePartClicked);
        }

        private async void AddSparePartClicked(object obj)
        {
            await Shell.Current.GoToAsync(nameof(TreeGroupingPage));
        }
    }
}
