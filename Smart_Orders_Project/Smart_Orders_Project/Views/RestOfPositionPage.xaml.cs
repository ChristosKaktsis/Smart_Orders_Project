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
    public partial class RestOfPositionPage : ContentPage
    {
        private RestOfPositionViewModel _viewModel;

        public RestOfPositionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new RestOfPositionViewModel();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(1000);
            Position_Text.Focus();
        }
        private async void Position_Text_Unfocused(object sender, FocusEventArgs e)
        {
            await _viewModel.LoadPosition(Position_Text.Text);
            await _viewModel.LoadProductsFromPosition(_viewModel.Position);
        }
    }
}