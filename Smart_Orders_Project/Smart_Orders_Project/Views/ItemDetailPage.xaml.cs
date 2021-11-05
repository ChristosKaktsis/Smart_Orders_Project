using Smart_Orders_Project.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Smart_Orders_Project.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}