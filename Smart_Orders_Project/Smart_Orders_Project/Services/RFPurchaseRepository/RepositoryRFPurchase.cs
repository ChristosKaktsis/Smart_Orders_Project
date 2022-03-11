using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    public class RepositoryRFPurchase
    {
        private List<RFPurchase> RFPurchaseList;
        private RFPurchase rFPurchase;

        public RepositoryRFPurchase()
        {
            RFPurchaseList = new List<RFPurchase>();
        }

        public async Task<List<RFPurchase>> GetItemsAsync()
        {
            //get items from db 
            await Task.Delay(5000);//work sim
            return await Task.FromResult(RFPurchaseList);
        }
        public async Task<RFPurchase> GetItemAsync(string id)
        {
            //get item from DB
            await Task.Delay(2000);//work sim
            return await Task.FromResult(rFPurchase);
        }
        public async Task<bool> AddItemAsync(RFPurchase item)
        {
            //add item to DB

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(RFPurchase item)
        {
            //update item from db

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            //delete item from db

            return await Task.FromResult(true);
        }
    }
}
