using SmartMobileWMS.Data;
using SmartMobileWMS.Models;
using System.Collections.Generic;

namespace SmartMobileWMS.ViewModels
{
    public class MainMenuViewModel
    {
        public MainMenuViewModel()
        {
            MenuItems = MenuItemsData.GetMenuItems();
        }

        public List<MenuItem> MenuItems { get; set; }
        
    }
}
