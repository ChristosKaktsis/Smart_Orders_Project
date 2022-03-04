using Smart_Orders_Project.Data;
using Smart_Orders_Project.Services;
using Xamarin.Forms;

namespace Smart_Orders_Project
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
        public App()
        {
            DevExpress.XamarinForms.CollectionView.Initializer.Init();
            DevExpress.XamarinForms.Editors.Initializer.Init();
            InitializeComponent();
            

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<RepositoryCustomers>();
            DependencyService.Register<RepositoryItems>();
            DependencyService.Register<RepositoryLineOfOrder>();
            DependencyService.Register<RepositoryRFSales>();
            DependencyService.Register<RepositoryRFStorage>();
            DependencyService.Register<RepositoryRFPosition>();
            DependencyService.Register<RepositoryRFCensus>();
            DependencyService.Register<RepositoryUser>();
            DependencyService.Register<RepositoryRecievers>();
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
