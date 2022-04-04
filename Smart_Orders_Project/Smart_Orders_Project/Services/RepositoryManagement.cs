using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    public class RepositoryManagement : RepositoryService
    {
        int result = 0;
        public async Task<bool> AddManagement(Management mangement)
        {
            if (mangement == null)
                return await Task.FromResult(false);
            if (mangement.Customer == null)
                return await Task.FromResult(false);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("postManagement"), mangement.Oid, mangement.Type,mangement.Customer.Oid,mangement.SalesDoc);
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
