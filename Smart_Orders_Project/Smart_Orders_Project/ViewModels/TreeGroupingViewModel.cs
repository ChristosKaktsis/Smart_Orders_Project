using Smart_Orders_Project.Models.SparePartModels;
using Smart_Orders_Project.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.ViewModels
{
    class TreeGroupingViewModel : BaseViewModel
    {
        RepositoryTreeGrouping repotree;
        List<Grouping> Groupinglist;
        public ObservableCollection<Grouping> GroupinglistFirstLayer { get; set; }
        private string _time;
        private Grouping _selectedGroup;

        public TreeGroupingViewModel()
        {
            repotree = new RepositoryTreeGrouping();
            Groupinglist = new List<Grouping>();
            GroupinglistFirstLayer = new ObservableCollection<Grouping>();
        }
        public async Task LoadGroupingItems()
        {
            try
            {
                IsBusy = true;
                Groupinglist.Clear();
                GroupinglistFirstLayer.Clear();
               var list = await repotree.GetItemsWithNameAsync("");
                foreach(var item in list)
                {
                    Groupinglist.Add(item);
                    if (string.IsNullOrWhiteSpace(item.ParentOid))
                        GroupinglistFirstLayer.Add(item);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public string Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }
        public Grouping SelectedGroup
        {
            get => _selectedGroup;
            set 
            { 
                SetProperty(ref _selectedGroup, value);
                if(value!=null)
                    SetChildrenList(value.Oid.ToString());
            } 
        }

        private async void SetChildrenList(string id)
        {
            try
            {
                IsBusy = true;
                GroupinglistFirstLayer.Clear();
                var list = await repotree.GetItemChildrenAsync(id);
                foreach (var item in list)
                    GroupinglistFirstLayer.Add(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
