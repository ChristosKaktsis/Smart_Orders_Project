using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    class RepositoryLineOfOrder : IDataStore<LineOfOrder>
    {
        public List<LineOfOrder> LineList;
        public RepositoryLineOfOrder()
        {
            LineList = new List<LineOfOrder>();
        }

        public async Task<bool> AddItemAsync(LineOfOrder item)
        {
            LineList.Add(item);

            return await Task.FromResult(true);
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<LineOfOrder> GetItemAsync(string id)
        {
            return await Task.FromResult(LineList.FirstOrDefault(s => s.Oid == Guid.Parse(id)));
        }

        public async Task<List<LineOfOrder>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(LineList);
        }

        public async Task<bool> UpdateItemAsync(LineOfOrder item)
        {
            var oldItem = LineList.Where((LineOfOrder arg) => arg.Oid == item.Oid).FirstOrDefault();
            LineList.Remove(oldItem);
            LineList.Add(item);

            return await Task.FromResult(true);
        }
    }
}
