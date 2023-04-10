using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    public class UserRepository : BaseRepository
    {
        public async Task<User> GetItemAsync(string username, string password)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getUser"), username);
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<User>(result);
        }
        public async Task<string> GetVAT()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getVAT"));
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            var vat = JsonSerializer.Deserialize<VAT>(result);
            return vat.vat;
        }
        class VAT
        {
            public string vat { get; set; }
        }
    }
}
