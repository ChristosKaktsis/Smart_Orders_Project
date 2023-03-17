using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    public class ManagementRepository:BaseRepository
    {
        public async Task<bool> AddItem(Management mangement)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("postManagement"), mangement.Oid, mangement.Type, mangement.Customer, mangement.SalesDoc);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }
        public async Task<Management> GetItemAsync(string doc)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getManagement"), doc);
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<Management>(result);
        }
        /// <summary>
        /// Delete all movements of a specific Management
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteItem(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("deleteMovement"), id);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }
    }
}
