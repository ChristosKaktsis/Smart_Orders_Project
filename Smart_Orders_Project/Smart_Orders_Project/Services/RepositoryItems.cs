using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    public class RepositoryItems : IDataStore<Product>
    {
        public RepositoryItems()
        {
            ProductsList = new List<Product>();
        }
        public List<Product> ProductsList { get; set; }

        public Task<bool> AddItemAsync(Product item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.Run(() =>
            {
                string queryString = "select Oid , Κωδικός ,Περιγραφή ,ΦΠΑ ,ΤιμήΧονδρικής from Είδος where GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ProductsList.Add(new Product
                        {
                            Oid = Guid.Parse(reader["Oid"].ToString()),
                            ProductCode = reader["Κωδικός"] != null ? reader["Κωδικός"].ToString() : string.Empty,
                            Name = reader["Περιγραφή"].ToString(),
                            FPA = int.Parse(reader["ΦΠΑ"]!=null ? reader["ΦΠΑ"].ToString(): "0"),
                            Price = double.Parse(reader["ΤιμήΧονδρικής"].ToString())
                        });
                    }
                    return ProductsList;
                }
            });
        }

        public Task<bool> UpdateItemAsync(Product item)
        {
            throw new NotImplementedException();
        }
        private string ConnectionString()
        {
            return @"User Id=sa;password=1;Pooling=false;Data Source=DESKTOP-DTOHJQR\SQLEXPRESS;Initial Catalog=maindemo";
        }
    }
}
