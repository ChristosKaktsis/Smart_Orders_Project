using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    public class RFPurchaseRepository : BaseRepository, IDatabase<RFPurchase>
    {
        public async Task<bool> AddItem(RFPurchase item)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("postPurchase"), item.Oid, item.Provider.Oid, item.ProviderDoc);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }

        public async Task<RFPurchase> GetItemAsync(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getPurchase"), id);
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<RFPurchase>(result);
        }

        public async Task<IEnumerable<RFPurchase>> GetItemsAsync()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getPurchases"));
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<IEnumerable<RFPurchase>>(result);
        }

        public async Task<bool> UpdateItem(RFPurchase item)
        {
            var itemToUpdate = await GetItemAsync(item.Oid.ToString());
            if (itemToUpdate == null) return false;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("putPurchase"), item.Provider.Oid, item.ProviderDoc, item.Complete, item.Oid);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }
    }
}
