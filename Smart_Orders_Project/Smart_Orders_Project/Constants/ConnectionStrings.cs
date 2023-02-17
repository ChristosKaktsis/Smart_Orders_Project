using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace SmartMobileWMS.Constants
{
    public class ConnectionStrings
    {
        public static string ConnectionString
        {
            get => Preferences.Get(nameof(ConnectionString), App.Current.Resources["ConnectionString"] as string);
            set
            {
                Preferences.Set(nameof(ConnectionString), value);
            }
        }
    }
}
