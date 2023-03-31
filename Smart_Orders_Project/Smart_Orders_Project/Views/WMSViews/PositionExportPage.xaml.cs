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
    public partial class PositionExportPage : ContentPage
    {
        private PositionBaseViewModel _viewModel;

        public PositionExportPage()
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
            if (_viewModel.IsPalette(Product_text.Text))
                GoForPalette();
            else
                GoForProduct();
        }
        private async void GoForPalette()
        {
            await _viewModel.FindPalette(_viewModel.ProductID);
            _viewModel.LoadContent();
            if(_viewModel.IsQuickOn) SavePalette();
        }
        private async void GoForProduct()
        {
            await _viewModel.SetProduct(_viewModel.ProductID);
            if (_viewModel.IsQuickOn)
                SaveProduct();
            else if (!string.IsNullOrWhiteSpace(Product_text.Text))
            {
                await Task.Delay(200);
                Quantity_text.Focus();
            }
        }
        private async void SavePalette()
        {
            await _viewModel.SavePaletteAtPosition(1);
            Reset();
        }
        private async void SaveProduct()
        {
            var answer = await PositionCheck();
            if (!answer)
                return;
            if (!await IsProductSNApproved())
            {
                Reset();
                return;
            }
            await _viewModel.ExecuteSavePosition(1);
            Reset();
        }
        private void Done_button_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.IsPalette(Product_text.Text))
                SavePalette();
            else
                SaveProduct();
        }
        private  void Reset()
        {
            //Done
            Product_text.Focus();
            Product_text.Text = string.Empty;
        }
        private async Task<bool> PositionCheck()
        {
            bool result = false;
            if (_viewModel.Position == null || _viewModel.Product == null)
                return result;
            result = await _viewModel.AnyProductLeft(_viewModel.Position.Oid.ToString(), _viewModel.Product.CodeDisplay, _viewModel.Quantity);
            if (!result)
            {
                bool answer = await DisplayAlert("Προσοχή", 
                    "Η ποσότητα που αφαιρείτε είναι μεγαλήτερη απο αυτή που έχει η θέση", 
                    "ΟΚ", "Άκυρο");
                    result = answer;
            }

            return result;
        }
        private async Task<bool> IsProductSNApproved()
        {
            if (!_viewModel.Product.SN) return true;
            if (!await _viewModel.ProductExistInPosition(_viewModel.Position.Oid.ToString(), _viewModel.Product.CodeDisplay))
            {
                await DisplayAlert("Serial Number",
                   $"Το είδος {_viewModel.Product.CodeDisplay} είναι Serial Number το οποίο δεν υπάρχει στη θέση ",
                   "ΟΚ");
                return false;
            }
            if (_viewModel.Quantity > 1)
            {
                await DisplayAlert("Ποσότητα Serial Number",
                $"Το είδος {_viewModel.Product.CodeDisplay} είναι Serial Number και δεν μπορείτε να εξάγετε ποσότητα μεγαλύτερη απο 1.",
                "ΟΚ");
                return false;
            }
            return true;
        }
        private void OpenPopUp_Button(object sender, EventArgs e)
        {
            OpenPopUp();
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            CartPopUp.IsOpen = !CartPopUp.IsOpen;
        }
    }
}