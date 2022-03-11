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
        private TreeGroupingViewModel _viewModel;

        public TreeGroupingPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new TreeGroupingViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var parent = ((Button)sender);
            
            _viewModel.DeleteSelectedCommand.Execute(null);
        }
    }
}