using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    internal interface IDatabase<T>
    {
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync();
        Task<bool> AddItem(T item);
        Task<bool> UpdateItem(T item);
    }
}
