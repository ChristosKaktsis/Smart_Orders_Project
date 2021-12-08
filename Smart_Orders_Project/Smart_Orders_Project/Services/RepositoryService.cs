using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Smart_Orders_Project.Services
{
    public class RepositoryService
    {
        public string ConnectionString
        {
            get => Preferences.Get(nameof(ConnectionString), @"User ID=sa;Password=1;Pooling=false;Data Source=192.168.1.187,1433\SQLEXPRESS2019;Initial Catalog=SmartLobSidall");
            set
            {
                Preferences.Set(nameof(ConnectionString), value);
            }
        }
    }
}
