using SmartMobileWMS.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace SmartMobileWMS.Views
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