using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Smart_Orders_Project.Services
{
    public class RepositoryRFCensus : IDataStore<RFCensus>
    {
        private string ConnectionString
        {
            get => Preferences.Get(nameof(ConnectionString), @"User Id=sa;password=1;Pooling=false;Data Source=192.168.3.44\SQLEXPRESS;Initial Catalog=maindemo");
        }
        List<RFCensus> RFCensusList;
        public RepositoryRFCensus()
        {
            RFCensusList = new List<RFCensus>();
        }
        public async Task<bool> AddItemAsync(RFCensus item)
        {
            RFCensusList.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = RFCensusList.Where((RFCensus arg) => arg.Oid.ToString() == id).FirstOrDefault();
            RFCensusList.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<RFCensus> GetItemAsync(string id)
        {
            return await Task.FromResult(RFCensusList.FirstOrDefault(s => s.Oid.ToString() == id));
        }

        public async Task<List<RFCensus>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(RFCensusList);
        }

        public Task<List<RFCensus>> GetItemsWithNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(RFCensus item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UploadItemAsync(RFCensus item)
        {
            throw new NotImplementedException();
        }
    }
}
