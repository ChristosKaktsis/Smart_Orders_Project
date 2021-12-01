using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Smart_Orders_Project.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<bool> DeleteItemFromDBAsync(string id);
        Task<T> GetItemAsync(string id);
        Task<List<T>> GetItemsAsync(bool forceRefresh = false);
        Task<List<T>> GetItemsWithNameAsync(string name);
        Task<bool> UploadItemAsync(T item);
        
    }
}
