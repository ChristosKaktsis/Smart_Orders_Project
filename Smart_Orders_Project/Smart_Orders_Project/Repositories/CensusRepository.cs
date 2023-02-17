using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    public class CensusRepository : BaseRepository, IDatabase<RFCensus>
    {
        public async Task<bool> AddItem(RFCensus item)
        {
            var lip = string.IsNullOrEmpty(item.Product.BarCode) ? "null" : $"'{item.Product.BarCode}'";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("postRFCensus"), item.Oid, item.Storage.Oid, item.Product.Oid, item.UserCreator.UserID, item.Quantity, item.Position.Oid, lip);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }

        public Task<RFCensus> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RFCensus>> GetItemsAsync()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the Census whith user id , storage id , position id 
        /// </summary>
        /// <param name="uid">User Oid</param>
        /// <param name="sid">Storage Oid</param>
        /// <param name="pid">Position Oid</param>
        /// <returns></returns>
        public async Task<IEnumerable<RFCensus>> GetItemsAsync(string uid,string sid,string pid)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getRFCensus"), uid, sid, pid);
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<IEnumerable<RFCensus>>(result).OrderBy(x=>x.CreationDate);
        }

        public Task<bool> UpdateItem(RFCensus item)
        {
            throw new NotImplementedException();
        }
    }
}
