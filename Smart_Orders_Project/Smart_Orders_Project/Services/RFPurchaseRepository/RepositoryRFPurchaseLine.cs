using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Smart_Orders_Project.Models;

namespace Smart_Orders_Project.Services
{
    public class RepositoryRFPurchaseLine
    {
        private List<RFPurchaseLine> RFPurchaseLines;
        private RFPurchaseLine rFPurchaseLine;

        public RepositoryRFPurchaseLine()
        {
            RFPurchaseLines = new List<RFPurchaseLine>();
        }

        public async Task<List<RFPurchaseLine>> GetItemsAsync()
        {
            //get items from db 
            await Task.Delay(5000);//work sim
            return await Task.FromResult(RFPurchaseLines);
        }
        public async Task<RFPurchaseLine> GetItemAsync(string id)
        {
            //get item from DB
            await Task.Delay(2000);//work sim
            return await Task.FromResult(rFPurchaseLine);
        }
        public async Task<bool> AddItemAsync(RFPurchaseLine item)
        {
            //add item to DB

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(RFPurchaseLine item)
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
