using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    public class RepositoryRFPurchase : RepositoryService
    {
        private List<RFPurchase> RFPurchaseList;
        private RFPurchase rFPurchase;
        int result = 0;
        public RepositoryRFPurchase()
        {
            RFPurchaseList = new List<RFPurchase>();
        }

        public async Task<List<RFPurchase>> GetItemsAsync()
        {
            //get items from db 
            RepositoryProvider repository = new RepositoryProvider();
            RFPurchaseList.Clear();
            string queryString = "SELECT Oid,Προμηθευτής,ΠαραστατικόΠρομηθευτή,Ολοκληρώθηκε,ΗμνίαΔημιουργίας FROM RFΑγορές where Ολοκληρώθηκε = 0 or Ολοκληρώθηκε is null and GCRecord is null";
            //StringBuilder sb = new StringBuilder();
            //sb.AppendFormat(await GetParamAsync("getRFSales"));
            //string queryString = sb.ToString();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    RFPurchaseList.Add(new RFPurchase
                    {
                        Oid = Guid.Parse(reader["Oid"].ToString()),
                        ProviderDoc = reader["ΠαραστατικόΠρομηθευτή"].ToString(),
                        Provider = await repository.GetItemAsync(reader["Προμηθευτής"].ToString()),
                        CreationDate = (DateTime)reader["ΗμνίαΔημιουργίας"],
                        Complete = reader["Ολοκληρώθηκε"] == DBNull.Value ? false : bool.Parse(reader["Ολοκληρώθηκε"].ToString())
                    });
                }
            }
                return await Task.FromResult(RFPurchaseList);
        }
        public async Task<RFPurchase> GetItemAsync(string id)
        {
            //get item from DB
            
            return await Task.FromResult(rFPurchase);
        }
        public async Task<bool> AddItemAsync(RFPurchase item)
        {
            //add item to DB
            

            string queryString = $@"INSERT INTO RFΑγορές (Oid, Προμηθευτής, ΠαραστατικόΠρομηθευτή, ΗμνίαΔημιουργίας)
                    VALUES((Convert(uniqueidentifier, N'{item.Oid}')), (Convert(uniqueidentifier, N'{item.Provider.Oid}')), '{item.ProviderDoc}', GETDATE());";
            //StringBuilder sb = new StringBuilder();
            //sb.AppendFormat(await GetParamAsync("postRFSale"), item.Oid, item.Customer.Oid, item.RFCount, lip);
            //string queryString = sb.ToString();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                result = await command.ExecuteNonQueryAsync();
            }
           
            return await Task.FromResult(result!=0);
        }

        public async Task<bool> UpdateItemAsync(RFPurchase item)
        {
            //update item from db
            string queryString = $@"UPDATE RFΑγορές SET Προμηθευτής = '{item.Provider.Oid}',ΠαραστατικόΠρομηθευτή = '{item.ProviderDoc}', Ολοκληρώθηκε = '{item.Complete}' , 
UpdSmart = '{item.Complete}' WHERE Oid = '{item.Oid}' ";

            //StringBuilder sb = new StringBuilder();
            //sb.AppendFormat(await GetParamAsync("putRFSales"), item.Customer.Oid, item.Complete, item.Complete, lip, item.Oid);
            //string queryString = sb.ToString();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                result = await command.ExecuteNonQueryAsync();
            }
            
            return await Task.FromResult(result!=0);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            //delete item from db

            return await Task.FromResult(true);
        }
    }
}
