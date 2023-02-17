using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartMobileWMS.Repositories
{
    public class CounterRepository : BaseRepository, IDatabase<Counter>
    {
        public async Task<bool> AddItem(Counter item)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("addCounter"),Guid.NewGuid());
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }

        public async Task<Counter> GetItemAsync(string id = "ΜετρητήςRF")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getRFCounter"));
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<Counter>(result);
        }

        public Task<IEnumerable<Counter>> GetItemsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateItem(Counter item)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("putRFCounter"), item.Value);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }
    }
}
