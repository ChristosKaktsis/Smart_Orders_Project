using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileWMS.Views.CustomViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageBarcodeScanner : ContentPage
    {
        Action<string> action;
        public ImageBarcodeScanner(Action<string> action)
        {
            InitializeComponent();
            this.action = action;
        }

        private void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.Navigation.PopAsync();
                action(result.ToString());
            });
        }
    }
}