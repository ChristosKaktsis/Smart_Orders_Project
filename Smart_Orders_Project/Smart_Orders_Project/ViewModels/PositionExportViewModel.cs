using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    public class PositionExportViewModel : PositionBaseViewModel
    {
        public PositionExportViewModel()
        {
            SavePositionCommand = new Command<int>(ExecuteSavePosition);
            im_ex = 1;
        }
        
    }
}
