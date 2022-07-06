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
    public partial class OrderDetailPage : ContentPage
    {
        private OrdersDetailViewModel _viewModel;

        public OrderDetailPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new OrdersDetailViewModel();
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
        void SwipeItem_Edit_Invoked(System.Object sender, SwipeItemTapEventArgs e)
        {

            //_viewModel.RFEdit.Execute(e.Item);
            
        }
        protected override bool OnBackButtonPressed()
        {
            //cancel navigation 
            //check if user wants to leave
            _viewModel.BackCommand.Execute(null);
            return true;
        }
    }
}