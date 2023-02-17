using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    public class StorageRepository:BaseRepository
    {
        public async Task<IEnumerable<Storage>> GetItemsAsync()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getAllStorage"));
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<IEnumerable<Storage>>(result);
        }
    }
}
