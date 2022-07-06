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
    public partial class ProviderSelectionPage : ContentPage
    {
        private ProviderSelectionViewModel _viewModel;

        public ProviderSelectionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ProviderSelectionViewModel();
        }

        private void ProviderCollection_Tap(object sender, DevExpress.XamarinForms.CollectionView.CollectionViewGestureEventArgs e)
        {
            if (e.Item != null)
            {
                _viewModel.AddProviderCommand.Execute(e.Item);
            }
        }
    }
}