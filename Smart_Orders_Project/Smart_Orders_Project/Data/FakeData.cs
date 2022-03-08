using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Data
{
    public class FakeData
    {
        private List<Provider> ProviderList;
        private List<RFPurchaseLine> RFPurchaseLines;
        private List<RFPurchase> RFPurchaseList;
        public FakeData()
        {
            ProviderList = new List<Provider>()
            {
                new Provider{ Oid = Guid.NewGuid(), AFM = "123123123", Name = "George", Email = "email@gmail.com", CodeNumber="33333"},
                new Provider{ Oid = Guid.NewGuid(), AFM = "34522222", Name = "John", Email = "email@gmail.com", CodeNumber="2"},
                new Provider{ Oid = Guid.NewGuid(), AFM = "5435345", Name = "Nick", Email = "email@gmail.com", CodeNumber="3"},
            };
            RFPurchaseLines = new List<RFPurchaseLine>();
            RFPurchaseList = new List<RFPurchase>();
        }
        public async Task<List<Provider>> GetProvidersAsync()
        {
            return await Task.FromResult(ProviderList);
        }
        public async Task<Provider> GetProviderAsync(string id)
        {
            var provider = ProviderList.Where(x=> x.Oid.ToString()==id).FirstOrDefault();
            return await Task.FromResult(provider);
        }
        public async Task<List<RFPurchase>> GetPurchasesAsync()
        {
            //get items from db 
            await Task.Delay(2000);//work sim
            return await Task.FromResult(RFPurchaseList);
        }
        public async Task<RFPurchase> GetPurchaseAsync(string id)
        {
            var rFPurchase = RFPurchaseList.Where(x => x.Oid.ToString() == id).FirstOrDefault();
            return await Task.FromResult(rFPurchase);
        }
        public async Task<bool> AddPurchaseAsync(RFPurchase item)
        {
            //add item to DB
            var purchase = new RFPurchase
            {
                Oid = item.Oid,
                Provider = item.Provider,
                ProviderDoc = item.ProviderDoc,
                Lines = item.Lines,
                Storage = item.Storage,
                CreationDate = item.CreationDate
            };
            RFPurchaseList.Add(purchase);
            return await Task.FromResult(true);
        }
        public async Task<bool> AddPurchaseLineAsync(RFPurchaseLine item)
        {
            //add item to DB
            var purchase = item;
            RFPurchaseLines.Add(purchase);
            return await Task.FromResult(true);
        }
        public async Task<List<RFPurchaseLine>> GetPurchaseLineAsync()
        {
            //get items from db 
            return await Task.FromResult(RFPurchaseLines);
        }
        public async Task<bool> DeletePurchaseLinesAsync()
        {
            RFPurchaseLines.Clear();

            return await Task.FromResult(true);
        }
    }
}
