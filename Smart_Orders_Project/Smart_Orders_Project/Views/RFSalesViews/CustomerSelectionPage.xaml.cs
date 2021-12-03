using Smart_Orders_Project.Models;
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
    public partial class CustomerSelectionPage : ContentPage
    {
        CustomerSelectionViewModel _viewModel;
        public CustomerSelectionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CustomerSelectionViewModel();
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            OrdersList.FilterString = "Contains([Name], '"+e.NewTextValue+"')";
        }

        private void OrdersList_Tap(object sender, DevExpress.XamarinForms.CollectionView.CollectionViewGestureEventArgs e)
        {
            if (e.Item != null)
            {
                _viewModel.SelectedCustomer = (Customer)e.Item;
                _viewModel.AddCustomerCommand.Execute(null);
            }
        }
    }
}