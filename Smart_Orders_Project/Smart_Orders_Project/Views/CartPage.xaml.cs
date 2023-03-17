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
    public partial class CartPage : ContentPage
    {
        private CartViewModel _viewModel;

        public CartPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CartViewModel();
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
            _viewModel.RemoveItem(e.Item as PositionChange);
        }

        private async void Position_TextEdit_Unfocused(object sender, FocusEventArgs e)
        {
            await _viewModel.GetPosition(Position_TextEdit.Text);
            Scan_Code_Edit.Focus();
        }
    }
}