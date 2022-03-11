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
    public partial class SparePartPage : ContentPage
    {
        private SparePartViewModel _viewModel;

        public SparePartPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SparePartViewModel();
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();

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