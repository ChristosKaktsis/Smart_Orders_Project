using SmartMobileWMS.ViewModels;
using SmartMobileWMS.Views.CustomViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileWMS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RFCesusPage : ContentPage
    {
        private RFCensusViewModel _viewModel;

        public RFCesusPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new RFCensusViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        private async void Scan_Code_Edit_Unfocused(object sender, FocusEventArgs e)
        {
            await _viewModel.GetProduct(Scan_Code_Edit.Text);
            Reset();
        }
        private void Reset()
        {
            if (string.IsNullOrWhiteSpace(Scan_Code_Edit.Text))
                return;
            Scan_Code_Edit.Text = string.Empty;
            Scan_Code_Edit.Focus();
        }
        private void SwipeItem_Invoked(object sender, DevExpress.XamarinForms.CollectionView.SwipeItemTapEventArgs e)
        {
        }

        private async void Position_TextEdit_Unfocused(object sender, FocusEventArgs e)
        {
            await _viewModel.GetPosition(Position_TextEdit.Text);
            Scan_Code_Edit.Focus();
        }
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            custom_popup.IsOpen = !custom_popup.IsOpen;
        }
        private async void custom_popup_TextChanged(object sender, TextChangedEventArgs e)
        {
            await _viewModel.SearchProduct(((ProductPopup)sender).Text);
        }
        private async void ImageButton_Clicked_1(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new ImageBarcodeScanner(doit));
        }
        private async void doit(string r)
        {
            await _viewModel.GetProduct(r);
        }
    }
}