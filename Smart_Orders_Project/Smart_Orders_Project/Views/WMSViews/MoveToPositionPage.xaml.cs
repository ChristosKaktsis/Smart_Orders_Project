using DevExpress.XamarinForms.Editors;
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
    public partial class MoveToPositionPage : ContentPage
    {
        private MoveToPositionViewModel _viewModel;
        public MoveToPositionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MoveToPositionViewModel();
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
            if (!string.IsNullOrWhiteSpace(Position_text.Text))
            {
                await Task.Delay(200);
                Product_text.Focus();
            }
        }
        private void Product_text_Unfocused(object sender, FocusEventArgs e)
        {
            if (_viewModel.IsPalette(Product_text.Text))
                GoForPalette();
            else
                GoForProduct();
        }
        private async void GoForPalette()
        {
            await _viewModel.FindPalette(_viewModel.ProductID);
            await _viewModel.LoadContent();
            PositionTo_text.Focus();
        }
        private async void GoForProduct()
        {
            await _viewModel.SetProduct(_viewModel.ProductID);
            if (Product_text.HasError)
                return;
            if (_viewModel.IsQuickOn)
                AddProduct();
            else if (!string.IsNullOrWhiteSpace(Product_text.Text))
            {
                await Task.Delay(200);
                Quantity_text.Focus();
            }
        }
        private async void AddProduct()
        {
            var answer = await PositionCheck();
            if (!answer)
                return;
            //Done
            _viewModel.AddProductToList(_viewModel.Product);
            Reset();
        }
        private void Reset()
        {
            Product_text.Focus();
            Product_text.Text = string.Empty;
        }
        private async Task<bool> PositionCheck()
        {
            bool result = false;
            if (_viewModel.Position == null || _viewModel.Product == null)
                return result;

            var pleft = await _viewModel.AnyProductLeft(_viewModel.Position.Oid.ToString(), _viewModel.Product.Oid.ToString(), _viewModel.Quantity);
            if (!pleft)
            {
                bool answer = await DisplayAlert("Προσοχή", "Η ποσότητα που αφαιρείτε είναι μεγαλήτερη απο αυτή που έχει η θέση", "ΟΚ", "Άκυρο");
                if (answer)
                    result = true;
            }
            else
            {
                result = true;
            }
            return result;
        }
        private void OpenPopUp_Button(object sender, EventArgs e)
        {
            OpenPopUp();
        }
        private void Add_Button_Clicked(object sender, EventArgs e)
        {
            AddProduct();
        }
        private async void PositionTo_text_Unfocused(object sender, FocusEventArgs e)
        {
            await _viewModel.SetPositionTo(_viewModel.PositionToID);
            if (_viewModel.IsPalette(Product_text.Text))
                await _viewModel.MovePaletteToPosition();
            else
                await _viewModel.MoveToPosition();

            ClearAll();
            Position_text.Focus();
        }
        private void ClearAll()
        {
            PositionTo_text.Text = string.Empty;
            Product_text.Text = string.Empty;
            Position_text.Text = string.Empty;
        }
        private void Close_ProductPopUp_Clicked(object sender, EventArgs e)
        {
            ProductPopUp.IsOpen = !ProductPopUp.IsOpen;
        }
        private async void PositionTo_text_Focused(object sender, FocusEventArgs e)
        {
            if (_viewModel.Position == null)
                await Shell.Current.DisplayAlert("Προσοχή", "Δέν επιλέξατε θέση από", "Οκ");
            if (!_viewModel.ProductList.Any() && !_viewModel.PaletteContent.Any())
                await Shell.Current.DisplayAlert("Προσοχή", "Δέν επιλέξατε κανένα είδος", "Οκ");
        }
    }
}