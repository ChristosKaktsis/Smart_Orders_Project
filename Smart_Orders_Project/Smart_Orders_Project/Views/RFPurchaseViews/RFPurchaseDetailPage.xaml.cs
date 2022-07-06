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
    public partial class RFPurchaseDetailPage : ContentPage
    {
        private RFPurchaseDetailViewModel _viewModel;

        public RFPurchaseDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new RFPurchaseDetailViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        void SwipeItem_Delete_Invoked(System.Object sender, SwipeItemTapEventArgs e)
        {
            _viewModel.DeleteCommand.Execute(e.Item);
        }
       
        void SwipeItem_Edit_Invoked(System.Object sender, SwipeItemTapEventArgs e)
        {

            //_viewModel.RFEdit.Execute(e.Item);

        }
    }
}