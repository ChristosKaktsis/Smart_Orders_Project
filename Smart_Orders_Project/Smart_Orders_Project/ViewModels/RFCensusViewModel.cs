using Smart_Orders_Project.Models;
using Smart_Orders_Project.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    class RFCensusViewModel : BaseViewModel
    {
        public ObservableCollection<RFCensus> RFCensusList { get; set; }
        public Command LoadItemsCommand { get; }
        public Command AddRFCensusCommand { get; }
        public RFCensusViewModel()
        {
            RFCensusList = new ObservableCollection<RFCensus>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddRFCensusCommand = new Command(OnAddRFCensusClicked);  
        }

        private async void OnAddRFCensusClicked(object obj)
        {
            await Shell.Current.GoToAsync(nameof(RFCensusDetailPage));
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                RFCensusList.Clear();
                var user = await UserRepo.GetUser();
                var items = await RFCensusRepo.GetItemsWithNameAsync(user.UserID.ToString());
                foreach (var item in items)
                {
                    RFCensusList.Add(item);
                }
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
        }
    }
}
