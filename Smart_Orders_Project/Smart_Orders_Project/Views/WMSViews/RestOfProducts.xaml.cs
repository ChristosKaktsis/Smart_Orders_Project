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
    public partial class RestOfProducts : ContentPage
    {
        private RestOfProductsViewModel _viewModel;

        public RestOfProducts()
        {
            InitializeComponent();
            BindingContext = _viewModel = new RestOfProductsViewModel();
        }
        public RestOfProducts(string pid)//we call this from FreePicking only
        {
            InitializeComponent();
            BindingContext = _viewModel = new RestOfProductsViewModel();
            Product_Text.Text = pid;
            ShowPositions();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(1000);
            if(string.IsNullOrWhiteSpace(Product_Text.Text))
                Product_Text.Focus();
        }
        private  void Product_Text_Unfocused(object sender, FocusEventArgs e)
        {
            ShowPositions();
        }

        private async void ShowPositions()
        {
            await _viewModel.SetProduct(Product_Text.Text);
            await _viewModel.LoadPositions();
        }
    }
}