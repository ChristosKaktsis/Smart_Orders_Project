using SmartMobileWMS.Constants;
using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZXing;

namespace SmartMobileWMS.Repositories
{
    public class PaletteRepository : BaseRepository, IDatabase<Palette>
    {
        public async Task<Palette> GetItemAsync(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getPalette"),id);
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<Palette>(result);
        }
        public Task<IEnumerable<Palette>> GetItemsAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<bool> AddItem(Palette item)
        {
            if (item == null)
                return false;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("postPalette"), item.Oid, item.SSCC, item.Description, item.Length, item.Width, item.Height);
            var result = await ExecutePostMethod(sb.ToString());
            if(result == 0) return false;
            return await UpdateItem(item);
        }
        public async Task<bool> UpdateItem(Palette item)
        {
            if (item == null || item.Products == null)
                return false;
            foreach(var product in item.Products)
            {
                var oid = Guid.NewGuid();
                var barcode = string.IsNullOrEmpty(product.BarCode) ? "null" : product.BarCode;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(await GetParamAsync("postPaletteItem"), oid, item.Oid, product.Oid, barcode, product.Quantity);
                var result = await ExecutePostMethod(sb.ToString());
            }
            return true;
        }
        public async Task<bool> DeleteItem(Palette item)
        {
            if (item == null)
                return false;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("deletePaletteContent"), item.Oid);
            var result = await ExecutePostMethod(sb.ToString());
            return result != 0;
        }
    }
}
