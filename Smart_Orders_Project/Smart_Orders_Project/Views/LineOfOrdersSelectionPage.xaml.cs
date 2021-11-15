﻿using Smart_Orders_Project.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Smart_Orders_Project.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LineOfOrdersSelectionPage : ContentPage
    {
        LineOfOrdersViewModel _viewModel;
        public LineOfOrdersSelectionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new LineOfOrdersViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        //private void grid_SelectionChanged(object sender, DevExpress.XamarinForms.CollectionView.CollectionViewSelectionChangedEventArgs e)
        //{
        //    if (e.DeselectedItems.Count == 1)
        //    {
        //        ((Product)e.DeselectedItems[0]).Ποσότητα = 0;

        //    }
        //    if (e.SelectedItems.Count == 1)
        //    {
        //        ((Product)e.SelectedItems[0]).Ποσότητα = (float)this.posotita.Value;

        //    }
        //    //  Console.WriteLine();
        //}
    }
}