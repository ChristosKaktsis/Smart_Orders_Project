using DevExpress.XamarinForms.CollectionView;
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
        void SwipeItem_Delete_Invoked(System.Object sender, SwipeItemTapEventArgs e)
        {

            //_viewModel.RFEdit.Execute(e.Item);
            _viewModel.DeleteCommand.Execute(e.Item);
        }
    }
}