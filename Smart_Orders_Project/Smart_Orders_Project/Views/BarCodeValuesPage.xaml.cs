﻿using SmartMobileWMS.ViewModels;
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
    public partial class BarCodeValuesPage : ContentPage
    {
        private BarCodeValuesPageViewModel _viewModel;

        public BarCodeValuesPage()
        {
            InitializeComponent();
            BindingContext = _viewModel =new BarCodeValuesPageViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}