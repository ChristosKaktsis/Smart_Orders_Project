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
    public partial class PositionExportPage : ContentPage
    {
        private PositionExportViewModel _viewModel;

        public PositionExportPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new PositionExportViewModel();
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
        private async void Product_text_Unfocused(object sender, FocusEventArgs e)
        {
            if (_viewModel.IsQuickOn)
                Reset();
            else if (!string.IsNullOrWhiteSpace(Product_text.Text))
            {
                await Task.Delay(200);
                Quantity_text.Focus();
            }
                
        }
        private void Done_button_Clicked(object sender, EventArgs e)
        {
            _viewModel.SavePositionCommand.Execute(1);
            Reset();
        }

        private void Reset()
        {
            //Done
            
            Product_text.Focus();
            Product_text.Text = string.Empty;
        }

        private void OpenPopUp_Button(object sender, EventArgs e)
        {
            OpenPopUp();
        }
    }
}