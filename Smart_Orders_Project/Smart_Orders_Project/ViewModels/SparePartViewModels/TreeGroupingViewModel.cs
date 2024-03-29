﻿using SmartMobileWMS.Models.SparePartModels;
using SmartMobileWMS.Services;
using SmartMobileWMS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    class TreeGroupingViewModel : BaseViewModel
    {
       
        List<Grouping> Groupinglist;
        public ObservableCollection<Grouping> SelectedGrouping { get; set; }
        public ObservableCollection<Grouping> GroupinglistFirstLayer { get; set; }
        public Command LoadItemsCommand { get; }
        public Command GoToDetailSparePartCommand { get; }
        public Command<Guid> DeleteSelectedCommand { get; }
        private string _time;
        private Grouping _selectedGroup;
        private float _height = 0;
        private float _helpCount = 60;
        private float _heightChildrenList;
        private bool _lastChoice;

        public TreeGroupingViewModel()
        {
            Groupinglist = new List<Grouping>();
            GroupinglistFirstLayer = new ObservableCollection<Grouping>();
            SelectedGrouping = new ObservableCollection<Grouping>();
            LoadItemsCommand = new Command(async () => await LoadGroupingItems());
            DeleteSelectedCommand = new Command<Guid>(OnDeleteSelectedClicked);
            GoToDetailSparePartCommand = new Command(OnGoToDetaiSparePart);
        }

        private void OnDeleteSelectedClicked(Guid id)
        {
            try
            {
                var oldItem = SelectedGrouping.Where(x => x.Oid == id).FirstOrDefault();
                var oldItemParent = SelectedGrouping.Where(x => x.Oid.ToString() == oldItem.ParentOid).FirstOrDefault();
                DeleteChildren(id);
                SelectedGrouping.Remove(oldItem);
                SetHeight();
                if (oldItemParent != null)
                {
                    SetChildrenList(oldItemParent.Oid.ToString());
                }
                else
                {
                    LoadItemsCommand.Execute(null);
                }
                
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void DeleteChildren(Guid id)
        {
            var child = SelectedGrouping.Where(x => x.ParentOid == id.ToString()).FirstOrDefault();
            if (child == null)
                return;
            DeleteChildren(child.Oid);
            SelectedGrouping.Remove(child);
        }

        public async Task LoadGroupingItems()
        {
            try
            {
                IsBusy = true;
                LastChoice = false; //disable the button
                Height = 0;
                _helpCount = 60;
                Groupinglist.Clear();
                SelectedGrouping.Clear();
                GroupinglistFirstLayer.Clear();
               var list = await GroupingRepo.GetItemsAsync();
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
                await Shell.Current.DisplayAlert("Σφάλμα!", "LoadGroupingItems \n" + ex.Message, "Οκ");
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
        public float Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }
        public float HeightChildrenList
        {
            get => _heightChildrenList;
            set => SetProperty(ref _heightChildrenList, value);
        }
        public Grouping SelectedGroup
        {
            get => _selectedGroup;
            set 
            { 
                SetProperty(ref _selectedGroup, value);
                if(value!=null)
                {
                    SetChildrenList(value.Oid.ToString());
                    SelectedGrouping.Add(value);
                    SetHeight();
                   // SelectedGroup = null;
                }               
            } 
        }
        public bool LastChoice
        {
            get => _lastChoice;
            set => SetProperty(ref _lastChoice, value);
        }
        private void SetHeight()
        {
            Height = 40;
            for (int i=0;i<SelectedGrouping.Count;i++)
            {
                Height += Height ;
            }
        }

        private async void SetChildrenList(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return;
                IsBusy = true;
                GroupinglistFirstLayer.Clear();
                var list = await GroupingRepo.GetItemChildrenAsync(id);
                LastChoice = !list.Any(); //set the button visible if is last item on list 
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
        public void OnAppearing()
        {
            IsBusy = true;
            LoadItemsCommand.Execute(null);
           
        }
        private async void OnGoToDetaiSparePart()
        {
            await Shell.Current.GoToAsync($"{nameof(NewSparePartPage)}?{nameof(NewSparePartViewModel.GroupId)}={SelectedGroup.Oid}");
        }
    }
}
