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
    public partial class TreeGroupingPage : ContentPage
    {
        private TreeGroupingViewModel _view;

        public TreeGroupingPage()
        {
            InitializeComponent();
            BindingContext = _view = new TreeGroupingViewModel();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _view.LoadGroupingItems();
        }
    }
}