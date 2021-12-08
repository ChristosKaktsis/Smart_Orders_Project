using Smart_Orders_Project.Models.SparePartModels;
using Smart_Orders_Project.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.ViewModels
{
    class TreeGroupingViewModel
    {
        RepositoryTreeGrouping repotree;
        List<Grouping> Groupinglist;
        public TreeGroupingViewModel()
        {
            repotree = new RepositoryTreeGrouping();
            Groupinglist = new List<Grouping>();
        }
        public async Task LoadGroupingItems()
        {
            try
            {
                await repotree.GetItemsWithNameAsync("");
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {

            }
           
        }
    }
}
