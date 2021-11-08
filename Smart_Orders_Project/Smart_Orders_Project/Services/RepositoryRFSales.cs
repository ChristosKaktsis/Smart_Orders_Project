using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    class RepositoryRFSales : IDataStore<RFSales>
    {
        public List<RFSales> RFSalesList;
        public RepositoryRFSales()
        {
            RFSalesList = new List<RFSales>();
        }

        public async Task<bool> AddItemAsync(RFSales item)
        {
            RFSalesList.Add(item);

            return await Task.FromResult(true);
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<RFSales> GetItemAsync(string id)
        {
            return await Task.FromResult(RFSalesList.FirstOrDefault(s => s.Oid == Guid.Parse(id)));
        }

        public async Task<List<RFSales>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(RFSalesList);
        }

        public async Task<bool> UpdateItemAsync(RFSales item)
        {
            var oldItem = RFSalesList.Where((RFSales arg) => arg.Oid == item.Oid).FirstOrDefault();
            RFSalesList.Remove(oldItem);
            RFSalesList.Add(item);
            return await Task.FromResult(true);
        }
        public async Task<bool> UploadItemAsync(RFSales item)
        {
            return await Task.Run(() =>
            {
                int ok = 0;
                string queryString = @"INSERT INTO RFΠωλήσεις (Oid, ΑποθηκευτικόςΧώρος, Πελάτης, ΠαραστατικάΠωλήσεων, ΠαραστατικόΠελάτη, 
                    Διαχείριση, UpdSmart, Ολοκληρώθηκε, ΗμνίαΔημιουργίας, ΑυτόματηΔιαγραφήΠαραστατικών, OptimisticLockField, GCRecord)
                    VALUES((Convert(uniqueidentifier, N'"+item.Oid+ "')), null, (Convert(uniqueidentifier, N'" + item.Customer.Oid + "')), null, null, null, '0', '0', (Convert(date, '" + item.CreationDate.ToString("MM/dd/yyyy") + "')), '0', '1', null); ";
                using (SqlConnection connection = new SqlConnection(ConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    ok = command.ExecuteNonQuery();
                }
                return ok <= 1 ? true : false ;
            });
        }
        private string ConnectionString()
        {
            return @"User Id=sa;password=1;Pooling=false;Data Source=192.168.3.44\SQLEXPRESS;Initial Catalog=maindemo";
        }
    }
}
