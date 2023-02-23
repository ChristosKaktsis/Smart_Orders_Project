using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    public class SalesDocRepository:BaseRepository
    {
        public async Task<SalesDoc> GetItemAsync(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getSalesDoc"),id);
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<SalesDoc>(result);
        }
    }
}
