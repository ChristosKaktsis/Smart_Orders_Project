using SmartMobileWMS.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;

namespace SmartMobileWMS.Repositories
{
    public class ProductRepository : BaseRepository
    {
        public async Task<Product> GetItemAsync(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getProductWithID"),id);
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            var items = JsonSerializer.Deserialize<IEnumerable<Product>>(result);
            return items.FirstOrDefault();
        }
    }
}
