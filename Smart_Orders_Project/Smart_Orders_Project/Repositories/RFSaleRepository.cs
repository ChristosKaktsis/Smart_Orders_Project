using SmartMobileWMS.Constants;
using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    public class RFSaleRepository : BaseRepository, IDatabase<RFSale>
    {
        public async Task<bool> AddItem(RFSale item)
        {
            var lip = (item.Reciever == null ? "null" : $"'{item.Reciever.Oid}'");
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("postRFSale"), item.Oid, item.Customer.Oid, item.RFCount, lip);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }

        public async Task<IEnumerable<RFSale>> GetItemsAsync()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getRFSales"));
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<IEnumerable<RFSale>>(result);
        }
        public async Task<RFSale> GetItemAsync(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getRFSale"),id);
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<RFSale>(result);
        }

        public async Task<bool> UpdateItem(RFSale item)
        {
            var itemToUpdate = await GetItemAsync(item.Oid.ToString());
            if (itemToUpdate == null) return false;
            //implement update
            var lip = (item.Reciever == null ? "null" : $"'{item.Reciever.Oid}'");

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("putRFSale"), item.Customer.Oid, item.Complete, item.Complete, lip, item.Oid);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }
    }
}
