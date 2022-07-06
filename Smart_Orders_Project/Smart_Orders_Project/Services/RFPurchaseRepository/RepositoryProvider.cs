using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using SmartMobileWMS.Models;

namespace SmartMobileWMS.Services
{
    public class RepositoryProvider : RepositoryService
    {
        private List<Provider> ProviderList;
        private Provider provider;

        public RepositoryProvider()
        {
            ProviderList = new List<Provider>();
        }

        public async Task<List<Provider>> GetItemsAsync(string name)
        {
            ProviderList.Clear();
            //get items from db 
            //string queryString = $"select Oid , Κωδικός ,Επωνυμία ,ΑΦΜ, Email from [Προμηθευτής] where (Επωνυμία like '%{name}%' or ΑΦΜ like '%{name}%') and GCRecord is null";

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getProviderWithName"),name);
            string queryString = sb.ToString();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    ProviderList.Add(new Provider
                    {
                        Oid = Guid.Parse(reader["Oid"].ToString()),
                        CodeNumber = reader["Κωδικός"] != null ? reader["Κωδικός"].ToString() : string.Empty,
                        Name = reader["Επωνυμία"].ToString(),
                        AFM = reader["ΑΦΜ"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
               
            }

            return await Task.FromResult(ProviderList);
        }
        public async Task<Provider> GetItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getProviderWithID"), id);
            string queryString = sb.ToString();
            //string queryString = $"select Oid , Κωδικός ,Επωνυμία ,ΑΦΜ, Email from [Προμηθευτής] where Oid = '{id}'";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                    return  null;
                
                reader.Read();

                provider = new Provider()
                {
                    Oid = Guid.Parse(reader["Oid"].ToString()),
                    CodeNumber = reader["Κωδικός"] != DBNull.Value ? reader["Κωδικός"].ToString() : string.Empty,
                    Name = reader["Επωνυμία"].ToString(),
                    AFM = reader["ΑΦΜ"].ToString(),
                    Email = reader["Email"].ToString()
                };
                
            }

            return await Task.FromResult(provider);
        }
    }
}
