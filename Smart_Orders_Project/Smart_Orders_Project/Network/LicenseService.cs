using SmartMobileWMS.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartMobileWMS.Network
{
    public class LicenseService : BaseApi
    {
        public async static Task<RestResult> GetLicense(string afm, string device_id)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                return null;
            if(string.IsNullOrWhiteSpace(afm) || string.IsNullOrWhiteSpace(device_id))
                return null;
            var client = await GetClient();
            string result = await client.GetStringAsync($"{Url}/License/{device_id}?afm={afm}");
            return JsonSerializer.Deserialize<RestResult>(result);
        }
        public async static Task<RestResult> AddLicense(string afm)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                return null;
            if (string.IsNullOrWhiteSpace(afm))
                return null;
            var client = await GetClient();
            var msg = new HttpRequestMessage(HttpMethod.Post, $"{Url}/License/{afm}");
            var response = await client.SendAsync(msg);
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<RestResult>(result);
        }
        public async static Task<RestResult> DeleteLicense(string afm, string device_id)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                return null;
            if (string.IsNullOrWhiteSpace(afm))
                return null;
            var client = await GetClient();
            var msg = new HttpRequestMessage(HttpMethod.Delete, $"{Url}/License/?afm={afm}&id={device_id}");
            var response = await client.SendAsync(msg);
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<RestResult>(result);
        }
    }
}
