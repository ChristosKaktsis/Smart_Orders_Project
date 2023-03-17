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
        public async Task<Product> GetItemAsync(string product_id, string position_id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getProductFromPosition"), position_id, product_id);
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<Product>(result);
        }
        /// <summary>
        /// Gets a list of products with position id
        /// </summary>
        /// <param name="id">Position Id</param>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetItemsAsync(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getProductsFromPosition"), id);
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<IEnumerable<Product>>(result);
        }
        public async Task<IEnumerable<Product>> SearchItemsAsync(string search)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getProductWithName"), search);
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<IEnumerable<Product>>(result);
        }
        public async Task<IEnumerable<ProductAndPosition>> GetItemsFromMovementAsync(string doc)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getItemsFromMovement"), doc);
            string result = await ExecuteGetMethod(sb.ToString());
            if (string.IsNullOrEmpty(result)) return null;
            return JsonSerializer.Deserialize<IEnumerable<ProductAndPosition>>(result);
        }
    }
    public class ProductAndPosition : Product
    {
        public Position Position { get; set; }
    }
}
