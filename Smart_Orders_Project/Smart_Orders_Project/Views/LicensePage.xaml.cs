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
    public partial class LicensePage : ContentPage
    {
        private LicenseViewModel _viewModel;

        public LicensePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new LicenseViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}