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
    public partial class PaletteEditPage : ContentPage
    {
        private PaletteEditViewModel _viewModel;
        private bool isDelete;
        public PaletteEditPage(Palette palette)
        {
            InitializeComponent();
            BindingContext = _viewModel = new PaletteEditViewModel(palette);
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await _viewModel.LoadContent();
        }
        private void Delete_Button_Clicked(object sender, EventArgs e)
        {
            var resource = App.Current.Resources["Tradic"];
            FrameColorChange(resource);
            SwitchToScan();
            isDelete = true;
        }
        private void Add_Button_Clicked(object sender, EventArgs e)
        {
            var resource = App.Current.Resources["Analogous"];
            FrameColorChange(resource);
            SwitchToScan();
            isDelete = false;
        }
        private async void SwitchToScan()
        {
            await Just_Text.TranslateTo(-500, 0, 100);
            Just_Text.IsVisible = false;
            Scan_Frame.IsVisible = true;
            Scan_Code_Edit.Focus();
        }

        private void FrameColorChange(object resource)
        {
            if (resource is Color color)
                Scan_Frame.BackgroundColor = color;
        }

        private async void Scan_Code_Edit_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Scan_Code_Edit.Text))
                return;

            if (!isDelete)
                await _viewModel.FindProduct(Scan_Code_Edit.Text);
            else
            {
                _viewModel.DeleteProduct(Scan_Code_Edit.Text);
                await Task.Delay(10);
            }
            Reset();
        }
        private void Reset()
        {
            if (string.IsNullOrWhiteSpace(Scan_Code_Edit.Text))
                return;
            Scan_Code_Edit.Text = string.Empty;
            Scan_Code_Edit.Focus();
        }

        private async void Finish_Button_Clicked(object sender, EventArgs e)
        {
            if (!await Answer())
                return;
            await _viewModel.Save();
            await Navigation.PopAsync();
        }
        private async Task<bool> Answer()
        {
            return await DisplayAlert("Αποθήκευση", "Θέλετε να αποθηκεύσετε τις αλλαγές ?", "Συνέχεια", "Άκυρο");
        }
    }
}