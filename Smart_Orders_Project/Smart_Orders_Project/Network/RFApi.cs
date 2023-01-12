using Newtonsoft.Json;
using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.Network
{
    internal class RFApi : BaseApi
    {
        public static async Task<List<RFCensus>> GetItemAsync(string id)
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync($"{Url}/{id}");
            return JsonConvert.DeserializeObject<List<RFCensus>>(result);
        }
    }
}
