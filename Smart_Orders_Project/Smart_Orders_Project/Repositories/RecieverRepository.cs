using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    internal class RecieverRepository : BaseRepository, IDatabase<Reciever>
    {
        public Task<bool> AddItem(Reciever item)
        {
            throw new NotImplementedException();
        }

        public Task<Reciever> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Reciever>> GetItemsAsync()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getRecievers"));
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<IEnumerable<Reciever>>(result);
        }

        public Task<bool> UpdateItem(Reciever item)
        {
            throw new NotImplementedException();
        }
    }
}
