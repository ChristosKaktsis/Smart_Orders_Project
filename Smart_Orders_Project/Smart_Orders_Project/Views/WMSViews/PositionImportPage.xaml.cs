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
    public partial class PositionImportPage : ContentPage
    {
        private PositionBaseViewModel _viewModel;

        public PositionImportPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new PositionBaseViewModel();
            OpenPopUp();
        }

        private async void OpenPopUp()
        {
            await Task.Delay(200);
            SpacePopUp.IsOpen = !SpacePopUp.IsOpen;
            _viewModel.OnAppearing();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            SpacePopUp.IsOpen = !SpacePopUp.IsOpen;
            if (!SpacePopUp.IsOpen)
                Position_text.Focus();
        }

        private async void Position_text_Unfocused(object sender, FocusEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(Position_text.Text))
            {
                await Task.Delay(200);
                Product_text.Focus();
            }
            
        }
        private  void Product_text_Unfocused(object sender, FocusEventArgs e)
        {
            if (IsPalette(_viewModel.ProductID))
                GoForPalette();
            else
                GoForProduct();
        }

        private async void GoForPalette()
        {
            await _viewModel.FindPalette(_viewModel.ProductID);
        }

        private async void GoForProduct()
        {
            await _viewModel.SetProduct(_viewModel.ProductID);
            if (_viewModel.IsQuickOn)
                Reset();
            else if (!string.IsNullOrWhiteSpace(Product_text.Text))
            {
                await Task.Delay(200);
                Quantity_text.Focus();
            }
        }

        private bool IsPalette(string productID)
        {
            return productID.StartsWith("111");
        }

        private void Done_button_Clicked(object sender, EventArgs e)
        {
            
            Reset();
        }

        private async void Reset()
        {
            //Done
            await _viewModel.ExecuteSavePosition(0);
            Product_text.Focus();
            Product_text.Text = string.Empty;
        }

        private void OpenPopUp_Button(object sender, EventArgs e)
        {
            OpenPopUp();
        }
    }
}