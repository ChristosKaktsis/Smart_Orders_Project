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
    public partial class PositionPage : ContentPage
    {
        private PositionViewModel _viewModel;
        private string HallErrorText = "Ο Διάδρομος δεν βρέθηκε";
        private string PositionErrorText = "Η θέση δεν βρέθηκε";
        public PositionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new PositionViewModel();
            Hallway_Edit.ErrorText = HallErrorText;
            Position_Edit.ErrorText = PositionErrorText;
        }

        private async void Start_Button_Clicked(object sender, EventArgs e)
        {
            await Start_Button.TranslateTo(-500, 0, 100);
            Start_Button.IsVisible = false;
            ScanHallway_Frame.IsVisible = true;
            await ScanHallway_Frame.TranslateTo(0, 0, 100);
            await Stop_Button.TranslateTo(0, 0, 100);
            Hallway_Edit.Focus();
        }

        private void Reset()
        {
            Position_Edit.Text = string.Empty;
            Position_Edit.Focus();
        }

        private async void Hallway_Edit_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Hallway_Edit.Text))
                return;
            await _viewModel.FindHallway(Hallway_Edit.Text);
            if(!(Hallway_Edit.HasError = _viewModel.CurrentHallway == null))
            {
                Hallway_Edit.IsReadOnly = true;
                await ScanPosition_Frame.TranslateTo(0, 0, 100);
                Reset();
            }
        }
        private async void Position_Edit_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Position_Edit.Text))
                return;
            await _viewModel.FindPosition(Position_Edit.Text);
            if(!(Position_Edit.HasError = _viewModel.CurrentPosition == null))
                _viewModel.AddPosiionToHallway(_viewModel.CurrentHallway, _viewModel.CurrentPosition);
            Reset();
        }
        private async void Stop_Button_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Τέλος", "Θέλετε να ολοκληρώσετε την διαδικασία εκμάθησης; Προσοχή θα διαγραφούν όλες οι προηγούμενες.", "Συνέχεια", "Άκυρο");
            if (answer)
            {
                _viewModel.AddHallway(_viewModel.CurrentHallway);
                await _viewModel.UpdateAll();
                await Navigation.PopAsync();
            }
        }

        private  void Next_Hall_Button_Clicked(object sender, EventArgs e)
        {
            _viewModel.AddHallway(_viewModel.CurrentHallway);
            ResetHall();
        }

        private  void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            ResetHall();
        }
        private async void ResetHall()
        {
            Hallway_Edit.IsReadOnly = false;
            await ScanPosition_Frame.TranslateTo(400, 0, 100);
            Hallway_Edit.Text = string.Empty;
            Hallway_Edit.Focus();
        }
    }
}