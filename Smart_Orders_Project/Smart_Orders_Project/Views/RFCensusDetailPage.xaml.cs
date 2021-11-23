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
    public partial class RFCensusDetailPage : ContentPage
    {
        private RFCensusDetailViewModel _viewModel;

        public RFCensusDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new RFCensusDetailViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();    
        }
    }
}