using Newtonsoft.Json;
using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.Network
{
    public class RFApi : BaseApi
    {
        public static async Task<IEnumerable<RFSale>> GetItemAsync()
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync($"{Url}/RF");
            return JsonConvert.DeserializeObject<IEnumerable<RFSale>>(result);
        }
    }
}
