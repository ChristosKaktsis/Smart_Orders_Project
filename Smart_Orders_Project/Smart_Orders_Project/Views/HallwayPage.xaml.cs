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
    public partial class HallwayPage : ContentPage
    {
        private HallWayViewModel _viewModel;
        private string errorText = "Δεν βρέθηκε ο Διάδρομος!";

        public HallwayPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HallWayViewModel();
            Hallway_Edit.ErrorText = errorText;
        }
        private async void Start_Button_Clicked(object sender, EventArgs e)
        {
            await Start_Button.TranslateTo(-500, 0, 100);
            Start_Button.IsVisible = false;
            Scan_Frame.IsVisible = true;
            await Scan_Frame.TranslateTo(0, 0, 100);
            await Info_Frame.TranslateTo(0, 0, 100);
            await Stop_Button.TranslateTo(0, 0, 100);
            Reset();
        }
        private async void Hallway_Edit_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Hallway_Edit.Text))
                return;
            await _viewModel.FindHallWay(Hallway_Edit.Text);
            Hallway_Edit.HasError = _viewModel.HallWay == null;
            _viewModel.AddHallway(_viewModel.HallWay);
            Reset();
        }

        private void Reset()
        {
            Hallway_Edit.Text = string.Empty;
            Hallway_Edit.Focus();
        }

        private async void Stop_Button_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Τέλος", "Θέλετε να ολοκληρώσετε την διαδικασία εκμάθησης; Προσοχή θα διαγραφούν όλες οι προηγούμενες.", "Συνέχεια", "Άκυρο");
            if (answer)
            {
                await _viewModel.UpdateHallWays();
                await Navigation.PopAsync();
            }
        }

        
    }
}