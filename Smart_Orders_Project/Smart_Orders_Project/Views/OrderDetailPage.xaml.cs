﻿using DevExpress.XamarinForms.CollectionView;
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
    }
}