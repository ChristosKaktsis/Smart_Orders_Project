using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Smart_Orders_Project.Services
{
    class RepositoryLineOfOrder : IDataStore<LineOfOrder>
    {
        public List<LineOfOrder> LineList;
        private string ConnectionString
        {
            get => Preferences.Get(nameof(ConnectionString), @"User Id=sa;password=1;Pooling=false;Data Source=192.168.3.44\SQLEXPRESS;Initial Catalog=maindemo");
        }
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
            return await Task.Run(() =>
            {
                int ok = 0;
                string queryString = @"INSERT INTO RFΓραμμέςΠωλήσεων (Oid, RFΠωλήσεις, Είδος, Ποσότητα, Θέση, 
                    OptimisticLockField, GCRecord, BarCodeΕίδους, ΠοσότηταΔιάστασης, Μήκος, Πλάτος, Υψος)
                    VALUES((Convert(uniqueidentifier, N'" + item.Oid + "')), (Convert(uniqueidentifier, N'" + item.RFSalesOid + "')), (Convert(uniqueidentifier, N'" + item.Product.Oid + "')), '"+item.Quantity+"', null, '1', null, "+(string.IsNullOrEmpty(item.Product.BarCode) ?"null":"'"+ item.Product.BarCode + "'")+", '" + item.Product.Price + "', '" + item.Length + "', '" + item.Width + "', '" + item.Height + "'); ";
               
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    ok = command.ExecuteNonQuery();
                }
                return ok >= 1 ? true : false;
            });
        }
        
    }
}
