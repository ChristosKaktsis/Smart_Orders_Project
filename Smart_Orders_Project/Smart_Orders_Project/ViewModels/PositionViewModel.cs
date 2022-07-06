using SmartMobileWMS.Models;
using SmartMobileWMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.ViewModels
{
    public class PositionViewModel : BaseViewModel
    {
        RepositoryPosition repositoryPosition;
        RepositoryHallway repositoryHallway;
        private Position _position;
        private Hallway _hallway;

        public List<Hallway> Hallways { get; set; }
        public PositionViewModel()
        {
            repositoryPosition = new RepositoryPosition();
            repositoryHallway = new RepositoryHallway();
            Hallways = new List<Hallway>();
            AAPicking = 1;
        }
        public Position CurrentPosition 
        { 
            get => _position; 
            set => SetProperty(ref _position, value); 
        }
        public Hallway CurrentHallway 
        { 
            get => _hallway;
            set 
            {
                SetProperty(ref _hallway, value);
                if (value != null)
                    value.Positions = new List<Position>();
            } 
        }
        public int AAPicking { get; set; }
        public async Task FindPosition(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;
            try
            {
                IsBusy = true;
                CurrentPosition = await repositoryPosition.GetPosition(id);
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
        public async Task FindHallway(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;
            try
            {
                IsBusy = true;
                CurrentHallway = await repositoryHallway.GetHallWay(id);
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
        public void AddPosiionToHallway(Hallway hallway, Position position)
        {
            if (hallway == null || position == null)
                return;
            if (hallway.Positions.Where(x => x.Oid == position.Oid).Any())
                return;
            foreach (var item in Hallways)
                if (item.Positions.Where(x => x.Oid == position.Oid).Any())
                    return;
            position.AAPicking = AAPicking;
            AAPicking++;
            hallway.Positions.Add(position);
        }
        public void AddHallway(Hallway hallway)
        {
            if (hallway == null)
                return;
            if(!Hallways.Where(x=>x.Oid == hallway.Oid).Any())
                Hallways.Add(hallway);
        }
        public async Task UpdateAll()
        {
            try
            {
                await repositoryPosition.SetAAToZero();
                foreach (var item in Hallways)
                    await UpdateHallwayPositions(item);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private async Task UpdateHallwayPositions(Hallway hallway)
        {
            try
            {
                if (hallway == null)
                    return;
                IsBusy = true;
                foreach(var item in hallway.Positions)
                    await repositoryPosition.UpdatePosition(item, hallway);
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
    }
}
