using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmartMobileWMS.Models
{
    public class MenuItem
    {
        public ImageSource ImageSource { get; set; }
        public string Title { get; set; }
        public Command Action { get; set; } = null;
    }
}
