using Smart_Orders_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Smart_Orders_Project.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarCodeScanner : ContentPage
    {
        private BarCodeScannerViewModel _viewModel;

        public BarCodeScanner()
        {
            InitializeComponent();
            BindingContext = _viewModel = new BarCodeScannerViewModel();
        }

        private void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            string res = "";
            Device.BeginInvokeOnMainThread(() =>
            {
                res = result.Text;
                //Console.WriteLine(res);
                _viewModel.BarCode = res;
            });
            
        }
    }
}