using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    public class RepositoryPositionChange : RepositoryService
    {
        int result = 0;
        public async Task<bool> PositionChange(Position position, Product product, int quantity, int impexp)
        {
            if (position == null || product == null)
                return await Task.FromResult(false);
            var Oid = Guid.NewGuid();
            var barcode = string.IsNullOrEmpty(product.BarCode) ? "null" : product.BarCode;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("postPosition"), Oid, position.Oid, barcode, product.Oid, quantity, impexp);
            string queryString = sb.ToString();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                result = command.ExecuteNonQuery();
            }
            return await Task.FromResult(result != 0);
        }
        public async Task<List<Product>> GetProductsFromList(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getProductsFromPosition"), id);
            string queryString = sb.ToString();
            string oldqueryString = $@"select * from (SELECT Είδος, BarCodeΕίδους, sum( IIF([ΤύποςΚίνησηςΘέσης]=1,[ΠοσότηταΕγγραφής]*-1,[ΠοσότηταΕγγραφής])) as Ποσότητα
FROM[maindemo].[dbo].[ΚίνησηΘέσης] where Θέση = '{id}' group by[Είδος], [BarCodeΕίδους] ) a left join
(SELECT[Είδος].[Oid],[Κωδικός],[Είδος].[Περιγραφή] FROM[maindemo].[dbo].[Είδος]) b on a.Είδος = b.Oid";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                /*ProductsList.Clear()*/
                List<Product> ProductsList = new List<Product>();
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    ProductsList.Add(new Product()
                    {
                        Oid = Guid.Parse(reader["Oid"].ToString()),
                        ProductCode = reader["Κωδικός"] != DBNull.Value ? reader["Κωδικός"].ToString() : string.Empty,
                        BarCode = reader["BarCodeΕίδους"] != DBNull.Value ? reader["BarCodeΕίδους"].ToString() : string.Empty,
                        Name = reader["Περιγραφή"].ToString(),
                        Quantity = int.Parse(reader["Ποσότητα"].ToString())
                    });
                }
                return await Task.FromResult(ProductsList);

            }
        }
    }
    
}
