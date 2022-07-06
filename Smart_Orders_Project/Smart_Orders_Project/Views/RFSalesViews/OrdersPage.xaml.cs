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
    public partial class OrdersPage : ContentPage
    {
        private OrdersViewModel _viewModel;

        public OrdersPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new OrdersViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        void SwipeItem_Edit_Invoked(System.Object sender, SwipeItemTapEventArgs e)
        {  
            _viewModel.RFEdit.Execute(e.Item);
        }
        void SwipeItem_Done_Invoked(System.Object sender, SwipeItemTapEventArgs e)
        {
            _viewModel.RFDone.Execute(e.Item);
        }
    }
}