using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.Services
{
    public class RepositoryColComand : RepositoryService
    {
        public async Task<List<CollectionCommand>> GetCollectionCommands(string pid)
        {
            if (string.IsNullOrEmpty(pid))
                return null;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getCollectionCommands"), pid);
            string queryString = sb.ToString();

            var Collection = new List<CollectionCommand>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Collection.Add(new CollectionCommand()
                    {
                        Oid = Guid.Parse(reader["Oid"].ToString()),
                        Product = new Product 
                        { 
                            Oid = Guid.Parse(reader["Είδος_Oid"].ToString()), 
                            Name = reader["Περιγραφή"].ToString(), 
                            ProductCode = reader["Κωδικός"].ToString(),
                            BarCode = reader["BarCodeΕίδους"].ToString()
                        },
                        Position = new Position
                        {
                            Oid = Guid.Parse(reader["Θέση"].ToString()),
                            PositionCode =  reader["Θέση_Κωδικός"].ToString(),
                            Description = reader["Θέση_Περιγραφή"].ToString()
                        },
                        Quantity = int.Parse(reader["ΠοσότηταΕντολής"].ToString())
                    });
                }
                return await Task.FromResult(Collection);

            }
        }
        public async Task<bool> UpdateCollectionCommand(CollectionCommand com)
        {
            int result = 0;

            if (com == null)
                return await Task.FromResult(false);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("putCollectedToCommand"), com.Collected, com.Oid);
            string queryString = sb.ToString();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                result = await command.ExecuteNonQueryAsync();
            }
            return await Task.FromResult(result != 0);
        }
    }
}
