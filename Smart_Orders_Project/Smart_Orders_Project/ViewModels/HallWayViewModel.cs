using Smart_Orders_Project.Models;
using Smart_Orders_Project.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.ViewModels
{
    public class HallWayViewModel : BaseViewModel
    {
        RepositoryHallway repositoryHall;
        private Hallway hal;
        private int aapicking;

        public List<Hallway> Hallways { get; set; }
        public HallWayViewModel()
        {
            repositoryHall = new RepositoryHallway();
            Hallways = new List<Hallway>();
            AAPicking = 1;
        }
        public Hallway HallWay 
        { 
            get=>hal; 
            set=>SetProperty(ref hal,value); 
        }
        public int AAPicking 
        {
            get => aapicking; 
            set => SetProperty(ref aapicking,value); 
        }
        public async Task FindHallWay(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return;
            try
            {
                IsBusy = true;
                HallWay = await repositoryHall.GetHallWay(id);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void AddHallway(Hallway hallway)
        {
            if (hallway == null)
                return;
            hallway.AAPicking = AAPicking;
            AAPicking++;
            Hallways.Add(hallway);
        }
        public async Task UpdateHallWays()
        {
            try
            {
                IsBusy = true;
                await repositoryHall.SetAAToZero();
                foreach(var item in Hallways)
                     await repositoryHall.UpdateHallWay(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
