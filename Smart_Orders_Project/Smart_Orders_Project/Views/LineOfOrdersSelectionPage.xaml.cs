using Smart_Orders_Project.ViewModels;
using System;
using System.Threading.Tasks;
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
            ChangeFocus();//open keyboard ready to write 
        }

        private async void ChangeFocus()
        {
            await Task.Delay(100);//delay because you have to wait for the element to render!!!
            await Task.Run(() =>
            {
                SearchText.Focus();
            });
        }
        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            OrdersList.FilterString = "";
            string[] subs = e.NewTextValue.Split(' ');
            switch (subs.Length)
            {
                case 1:
                    break;
                case 2:
                    OrdersList.FilterString = "Contains([Name], '" + subs[1] + "')";
                    break;
                default:      
                    break;
            }
            
            
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