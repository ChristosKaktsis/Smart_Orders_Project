using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Smart_Orders_Project.Models;

namespace Smart_Orders_Project.Services
{
    public class RepositoryProvider
    {
        private List<Provider> ProviderList;
        private Provider provider;

        public RepositoryProvider()
        {
            ProviderList = new List<Provider>();
        }

        public async Task<List<Provider>> GetItemsAsync()
        {
            //get items from db 
            await Task.Delay(5000);//work sim
            var list = await App.Database.GetProvidersAsync();
            foreach (var item in list)
                ProviderList.Add(item);

            return await Task.FromResult(ProviderList);
        }
        public async Task<Provider> GetItemAsync(string id)
        {
            //get item from DB
            await Task.Delay(2000);//work sim
            var pro = await App.Database.GetProviderAsync(id);

            return await Task.FromResult(pro);
        }
    }
}
