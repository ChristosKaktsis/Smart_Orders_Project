using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace SmartMobileWMS.Constants
{
    internal static class InfoStrings
    {
        public static string Device_ID //Device id for license
        {
            get => Preferences.Get(nameof(Device_ID), string.Empty);
            set
            {
                Preferences.Set(nameof(Device_ID), value);
            }
        }
    }
}
