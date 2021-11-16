using Smart_Orders_Project.ViewModels;
using Smart_Orders_Project.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Smart_Orders_Project
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(OrderDetailPage), typeof(OrderDetailPage));
            Routing.RegisterRoute(nameof(CustomerSelectionPage), typeof(CustomerSelectionPage));
            Routing.RegisterRoute(nameof(LineOfOrdersSelectionPage), typeof(LineOfOrdersSelectionPage));
            Routing.RegisterRoute(nameof(BarCodeScanner), typeof(BarCodeScanner));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
