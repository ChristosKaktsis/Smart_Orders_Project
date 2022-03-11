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
        private PositionImportViewModel _viewModel;

        public PositionImportPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new PositionImportViewModel();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
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
            if (!SpacePopUp.IsOpen)
            {
                await Task.Delay(200);
                Product_text.Focus();
            }
                
        }

        private void Done_button_Clicked(object sender, EventArgs e)
        {
            Reset();
        }

        private void Product_text_Unfocused(object sender, FocusEventArgs e)
        {
            if(_viewModel.IsQuickOn)
            {
                Reset();
            }
        }
        private void Reset()
        {
            //Done
            Product_text.Focus();
            Product_text.Text = string.Empty;
        }
    }
}