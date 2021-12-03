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
    public partial class RFCesusPage : ContentPage
    {
        private RFCensusViewModel _viewModel;

        public RFCesusPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new RFCensusViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void SwipeItem_Invoked(object sender, DevExpress.XamarinForms.CollectionView.SwipeItemTapEventArgs e)
        {
            _viewModel.DeleteCommand.Execute(e.Item);
        }
    }
}