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
    public partial class PaletteStartPage : ContentPage
    {
        private PaletteStartViewModel _viewModel;

        public PaletteStartPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new PaletteStartViewModel();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PalettePage());
        }

        private async void Search_Edit_Unfocused(object sender, FocusEventArgs e)
        {
            await _viewModel.FindPalette(Search_Edit.Text);
        }

        private async void Delete_button_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Αποπαλετοποίηση", "Θέλετε να γίνει αποπαλετοποίηση ;", "Συνέχεια", "'Ακυρο");
            if(answer)
                await _viewModel.DeletePaletteContent();
        }

        private async void Edit_button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaletteEditPage(_viewModel.Palette));
        }
    }
}