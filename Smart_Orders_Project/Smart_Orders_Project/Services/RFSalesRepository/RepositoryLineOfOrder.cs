using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartMobileWMS.Services
{
    class RepositoryLineOfOrder : RepositoryService, IDataStore<LineOfOrder>
    {
        public List<LineOfOrder> LineList;
       
        public RepositoryLineOfOrder()
        {
            LineList = new List<LineOfOrder>();
        }

        public async Task<bool> AddItemAsync(LineOfOrder item)
        {
            LineList.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = LineList.Where((LineOfOrder arg) => arg.Oid == Guid.Parse(id)).FirstOrDefault();
            LineList.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<LineOfOrder> GetItemAsync(string id)
        {
            return await Task.FromResult(LineList.FirstOrDefault(s => s.Oid == Guid.Parse(id)));
        }

        public async Task<List<LineOfOrder>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(LineList);
        }
        public async Task<List<LineOfOrder>> GetItemsDBAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(LineList);
        }

        public Task<List<LineOfOrder>> GetItemsWithNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateItemAsync(LineOfOrder item)
        {
            var oldItem = LineList.Where((LineOfOrder arg) => arg.Oid == item.Oid).FirstOrDefault();
            LineList.Remove(oldItem);
            LineList.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UploadItemAsync(LineOfOrder item)
        {
            return await Task.Run(async() =>
            {
                string q = item.Quantity.ToString().Replace(',', '.');
                string h = item.Height.ToString().Replace(',', '.');
                string l = item.Length.ToString().Replace(',', '.');
                string w = item.Width.ToString().Replace(',', '.');
                var lip = string.IsNullOrEmpty(item.Product.BarCode) ? "null" : "'" + item.Product.BarCode + "'";

                int ok = 0;
                string oldqueryString = $@"INSERT INTO RFΓραμμέςΠωλήσεων (Oid, RFΠωλήσεις, Είδος, Ποσότητα, Θέση, 
                    OptimisticLockField, GCRecord, BarCodeΕίδους, ΠοσότηταΔιάστασης, Μήκος, Πλάτος, Υψος)
                    VALUES((Convert(uniqueidentifier, N'{ item.Oid }')), 
                    (Convert(uniqueidentifier, N'{ item.RFSalesOid }')), 
                    (Convert(uniqueidentifier, N'{ item.Product.Oid }')),
                    Convert(float,'{q}'), null, '1', null, "+lip+", Convert(float,'" + q + "'), Convert(float,'" + l + "'), Convert(float,'" + w + "'), Convert(float,'" + h + "')); ";
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(await GetParamAsync("postRFLine"), item.Oid, item.RFSalesOid, item.Product.Oid, q, lip, q, l, w, h);
                string queryString = sb.ToString();

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    ok = command.ExecuteNonQuery();
                }
                return ok >= 1 ? true : false;
            });
        }

        public Task<bool> DeleteItemFromDBAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
