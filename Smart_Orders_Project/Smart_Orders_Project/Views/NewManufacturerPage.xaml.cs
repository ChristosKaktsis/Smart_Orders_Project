﻿using Smart_Orders_Project.ViewModels;
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
	public partial class NewManufacturerPage : ContentPage
	{
        private NewManufacturerViewModel _viewModel;

        public NewManufacturerPage ()
		{
			InitializeComponent ();
			BindingContext = _viewModel = new NewManufacturerViewModel();
		}
	}
}