using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    public class RepositorySalesDoc :RepositoryService
    {
        public async Task<SalesDoc> getSalesDoc(string doc)
        {
            if (string.IsNullOrEmpty(doc))
                return null;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getSalesDoc"),doc);
            string queryString = sb.ToString();
           

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                /*ProductsList.Clear()*/

                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                    return null;
                reader.Read();
                SalesDoc salesDoc = new SalesDoc()
                {
                    Oid = Guid.Parse(reader["Oid"].ToString()),
                    Customer = new Customer { Oid = Guid.Parse(reader["Πελάτης"].ToString()) },
                    Doc = reader["Παραστατικό"].ToString()
                };
                return await Task.FromResult(salesDoc);

            }
        }
    }
}
