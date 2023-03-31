using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    public class CollectionCommandRepository : BaseRepository
    {
        public async Task<CCommand> GetItemsAsync(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getCollectionCommands"), id);
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<CCommand>(result);
        }
        public async Task<bool> UpdateItem(CollectionCommand item)
        {
            if (item == null)
                return await Task.FromResult(false);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("putCollectedToCommand"), item.Collected, item.Oid);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }
        public async Task<bool> AddItem(CollectionCommand item, string commandId) 
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(await GetParamAsync("postCollectedToCommand"), 
                    item.Oid,
                    item.Position.Oid,
                    item.Product.BarCode,
                    item.Product.Oid,
                    item.Quantity,
                    item.Collected,
                    commandId,
                    item.ParentId);
                var result = await ExecutePostMethod(sb.ToString());
                return result != 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
