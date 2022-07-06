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
    public partial class NewRecieverPage : ContentPage
    {
        public NewRecieverPage()
        {
            InitializeComponent();
            BindingContext = new NewRecieverViewModel();
        }
    }
}