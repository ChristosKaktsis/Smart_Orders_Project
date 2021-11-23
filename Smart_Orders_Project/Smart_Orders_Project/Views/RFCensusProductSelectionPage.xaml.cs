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
    public partial class RFCensusProductSelectionPage : ContentPage
    {
        RFCensusProductSelectionViewModel _viewModel;
        public RFCensusProductSelectionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new RFCensusProductSelectionViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
            ChangeFocus();//open keyboard ready to write 
        }

        private async void ChangeFocus()
        {
            await Task.Delay(100);//delay because you have to wait for the element to render!!!
            await Task.Run(() =>
            {
                SearchText.Focus();
            });
        }
        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            OrdersList.FilterString = "";
            string[] subs = e.NewTextValue.Split(' ');
            switch (subs.Length)
            {
                case 1:
                    break;
                case 2:
                    OrdersList.FilterString = "Contains([Name], '" + subs[1] + "')";
                    break;
                default:
                    break;
            }
        }

    }
}