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
    public partial class OrderDetailPage : ContentPage
    {
        private OrdersDetailViewModel _viewModel;

        public OrderDetailPage(RFSale rf = null)
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new OrdersDetailViewModel(rf);
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
        protected override bool OnBackButtonPressed()
        {
            //cancel navigation 
            //check if user wants to leave
            return true;
        }

        private void Customer_popup_Button_Clicked(object sender, EventArgs e)
        {
            customer_popup.IsOpen = !customer_popup.IsOpen;
        }

        private async void Search_TextChanged(object sender, EventArgs e)
        {
            await _viewModel.GetCustomers(Search.Text);
        }

        private void SwipeItem_Invoked(object sender, SwipeItemTapEventArgs e)
        {
            _viewModel.DeleteLine(e.Item as LineOfOrder);
        }

        private void NumericEdit_Unfocused(object sender, FocusEventArgs e)
        {
            Scan_Code_Edit.Focus();
        }
    }
}