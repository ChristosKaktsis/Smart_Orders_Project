using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    public class LineRepository : BaseRepository, IDatabase<LineOfOrder>
    {
        public async Task<bool> AddItem(LineOfOrder item)
        {
            string q = item.Quantity.ToString().Replace(',', '.');
            string h = item.Height.ToString().Replace(',', '.');
            string l = item.Length.ToString().Replace(',', '.');
            string w = item.Width.ToString().Replace(',', '.');
            var lip = string.IsNullOrEmpty(item.Product.BarCode) ? "null" : "'" + item.Product.BarCode + "'";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("postRFLine"), item.Oid, item.RFSalesOid, item.Product.Oid, q, lip, q, l, w, h);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }

        public Task<LineOfOrder> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }
         
        public Task<IEnumerable<LineOfOrder>> GetItemsAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<LineOfOrder>> GetItemsAsync(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getRFLinesWithRFSaleID"),id);
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<IEnumerable<LineOfOrder>>(result);
        }

        public Task<bool> UpdateItem(LineOfOrder item)
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
            sb.AppendFormat(await GetParamAsync("deleteRFLineWithID"),id);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }
    }
}
