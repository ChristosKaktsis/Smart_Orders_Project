using SmartMobileWMS.Constants;
using SmartMobileWMS.Data;
using SmartMobileWMS.Models;
using SmartMobileWMS.Services;
using System;
using Xamarin.Forms;

namespace SmartMobileWMS
{
    public partial class App : Application
    {
        public static User User { get; set; }
        public App()
        {
            DevExpress.XamarinForms.Editors.Initializer.Init();
            DevExpress.XamarinForms.CollectionView.Initializer.Init();
            DevExpress.XamarinForms.Popup.Initializer.Init();
            InitializeComponent();

            DependencyService.Register<RepositoryBrand>();
            DependencyService.Register<RepositoryManufacturer>();
            DependencyService.Register<RepositoryTreeGrouping>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        
    }
}
