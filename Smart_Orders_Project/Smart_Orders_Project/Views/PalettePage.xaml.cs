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
    public partial class PalettePage : ContentPage
    {
        private PaletteBuildViewModel _viewModel;

        public PalettePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new PaletteBuildViewModel();
        }

        private async void Scan_Code_Edit_Unfocused(object sender, FocusEventArgs e)
        {
            await _viewModel.FindProduct(Scan_Code_Edit.Text);
            Reset();
        }
        private void Reset()
        {
            if (string.IsNullOrWhiteSpace(Scan_Code_Edit.Text))
                return;
            Scan_Code_Edit.Text = string.Empty;
            Scan_Code_Edit.Focus();
        }
        private void SwipeItem_Invoked(object sender, DevExpress.XamarinForms.CollectionView.SwipeItemTapEventArgs e)
        {
            _viewModel.DeleteProduct(e.Item as Product);
        }

        private async void Start_Button_Clicked(object sender, EventArgs e)
        {
            await Start_Button.TranslateTo(-500, 0, 100);
            Start_Button.IsVisible = false;
            Scan_Frame.IsVisible = true;
            Scan_Code_Edit.Focus();
        }

        private async void Finish_Clicked(object sender, EventArgs e)
        {
            if (!await Answer())
                return;
            CheckForEmpty();
            if (IsSMTHEmpty())
                return;
            await _viewModel.SavePalette();
        }

        private async Task<bool> Answer()
        {
            return await DisplayAlert("Αποθήκευση", "Θέλετε να ολοκληρώσετε την παλετοποίηση ?", "Συνέχεια", "Άκυρο");
        }

        private void CheckForEmpty()
        {
            CheckSSCCForErrors();
            Description_edit.HasError = string.IsNullOrWhiteSpace(Description_edit.Text);
        }

        private bool IsSMTHEmpty()
        {
            return SSCC_Edit.HasError || Description_edit.HasError;
        }

        private void SSCC_Edit_Unfocused(object sender, FocusEventArgs e)
        {
            CheckSSCCForErrors();
        }
        private async void CheckSSCCForErrors()
        {
            await _viewModel.FindPalette(SSCC_Edit.Text);
            if(SSCC_Edit.HasError = _viewModel.SSCCHasError)
                SSCC_Edit.ErrorText = "Το SSCC υπάρχει";
            else
            {
                SSCC_Edit.HasError = string.IsNullOrWhiteSpace(SSCC_Edit.Text);
                SSCC_Edit.ErrorText = string.Empty;
            }
        }
    }
}