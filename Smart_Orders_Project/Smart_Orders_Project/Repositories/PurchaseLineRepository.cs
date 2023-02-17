using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    public class PurchaseLineRepository : BaseRepository, IDatabase<RFPurchaseLine>
    {
        public async Task<bool> AddItem(RFPurchaseLine item)
        {
            string q = item.Quantity.ToString().Replace(',', '.');
            string h = item.Height.ToString().Replace(',', '.');
            string l = item.Length.ToString().Replace(',', '.');
            string w = item.Width.ToString().Replace(',', '.');
            var lip = string.IsNullOrEmpty(item.Product.BarCode) ? "null" : "'" + item.Product.BarCode + "'";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("postPurchaseLine"), item.Oid, item.RFSalesOid, item.Product.Oid, q, lip, q, l, w, h);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }

        public Task<RFPurchaseLine> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RFPurchaseLine>> GetItemsAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<RFPurchaseLine>> GetItemsAsync(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getPurchaseLines"), id);
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<IEnumerable<RFPurchaseLine>>(result);
        }
        public Task<bool> UpdateItem(RFPurchaseLine item)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Deletes all the lines in the same RFSALE
        /// </summary>
        /// <param name="id">The Oid of RFSale</param>
        /// <returns></returns>
        public async Task<bool> DeleteItems(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("deletePurchaseLine"), id);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }
    }
}
