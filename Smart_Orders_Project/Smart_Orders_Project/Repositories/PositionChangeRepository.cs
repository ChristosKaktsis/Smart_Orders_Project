using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    public class PositionChangeRepository: BaseRepository
    {
        public async Task<bool> AddItem(PositionChange item)
        {
            if (item == null) return false;
            var Oid = Guid.NewGuid();
            var barcode = string.IsNullOrEmpty(item.Product.BarCode) ? "null" : item.Product.BarCode;
            string m = item.Management == null ? "null" : $"'{item.Management.Oid}'";//if null send null else send 'guid'
            string pal = item.Palette == null ? "null" : $"'{item.Palette.Oid}'";//if null send null else send 'guid'

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("postPosition"), Oid, item.Position.Oid, barcode, item.Product.Oid, item.Quantity, item.Type, m, pal);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }
    }
}
