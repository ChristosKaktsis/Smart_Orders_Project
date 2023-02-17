using DevExpress.XamarinForms.CollectionView;
using SmartMobileWMS.Models;
using SmartMobileWMS.ViewModels;
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
    public partial class RFPurchaseDetailPage : ContentPage
    {
        private RFPurchaseDetailViewModel _viewModel;

        public RFPurchaseDetailPage(RFPurchase rf = null)
        {
            InitializeComponent();
            BindingContext = _viewModel = new RFPurchaseDetailViewModel(rf);
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

        private async void Search_TextChanged(object sender, EventArgs e)
        {
            await _viewModel.GetProviders(Search.Text);
        }

        private void SwipeItem_Invoked(object sender, SwipeItemTapEventArgs e)
        {
            _viewModel.DeleteLine(e.Item as RFPurchaseLine);
        }

        private void NumericEdit_Unfocused(object sender, FocusEventArgs e)
        {
            Scan_Code_Edit.Focus();
        }
    }
}