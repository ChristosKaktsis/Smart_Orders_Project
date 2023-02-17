using SmartMobileWMS.Constants;
using SmartMobileWMS.Data;
using SmartMobileWMS.Services;
using System;
using Xamarin.Forms;

namespace SmartMobileWMS
{
    public partial class App : Application
    {
        static FakeData database;

        // Create the database connection as a singleton.
        public static FakeData Database
        {
            get
            {
                if (database == null)
                {
                    database = new FakeData();
                }
                return database;
            }
        }
        static Database _database;

        public static Database _Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new Database(ConnectionStrings.ConnectionString);
                }
                return _database;
            }
        }
        public App()
        {
            DevExpress.XamarinForms.Editors.Initializer.Init();
            DevExpress.XamarinForms.CollectionView.Initializer.Init();
            DevExpress.XamarinForms.Popup.Initializer.Init();
            InitializeComponent();

            DependencyService.Register<RepositoryItems>();
            DependencyService.Register<RepositoryRFStorage>();
            DependencyService.Register<RepositoryRFPosition>();
            DependencyService.Register<RepositoryUser>();
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
