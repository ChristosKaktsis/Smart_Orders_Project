using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    public class ManagementRepository:BaseRepository
    {
        public async Task<bool> AddItem(Management mangement)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("postManagement"), mangement.Oid, mangement.Type, mangement.Customer.Oid, mangement.SalesDoc);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }
    }
}
