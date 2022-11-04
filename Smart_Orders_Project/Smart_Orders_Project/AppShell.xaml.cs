using SmartMobileWMS.ViewModels;
using SmartMobileWMS.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SmartMobileWMS
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(OrderDetailPage), typeof(OrderDetailPage));
            Routing.RegisterRoute(nameof(CustomerSelectionPage), typeof(CustomerSelectionPage));
            Routing.RegisterRoute(nameof(LineOfOrdersSelectionPage), typeof(LineOfOrdersSelectionPage));
            Routing.RegisterRoute(nameof(BarCodeScanner), typeof(BarCodeScanner));
            Routing.RegisterRoute(nameof(RFCensusDetailPage), typeof(RFCensusDetailPage));
            Routing.RegisterRoute(nameof(RFCensusProductSelectionPage), typeof(RFCensusProductSelectionPage));
            Routing.RegisterRoute(nameof(RFPurchaseDetailPage), typeof(RFPurchaseDetailPage));
            Routing.RegisterRoute(nameof(RFPurchaseLineSelectionPage), typeof(RFPurchaseLineSelectionPage));
            Routing.RegisterRoute(nameof(ProviderSelectionPage), typeof(ProviderSelectionPage));
            Routing.RegisterRoute(nameof(NewRecieverPage), typeof(NewRecieverPage));
            Routing.RegisterRoute(nameof(TreeGroupingPage), typeof(TreeGroupingPage));
            Routing.RegisterRoute(nameof(NewSparePartPage), typeof(NewSparePartPage));
            Routing.RegisterRoute(nameof(NewManufacturerPage), typeof(NewManufacturerPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
