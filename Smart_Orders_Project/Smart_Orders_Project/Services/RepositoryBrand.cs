using Smart_Orders_Project.Models.SparePartModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    public class RepositoryBrand :RepositoryService,IDataStore<Brand>
    {
        List<Brand> BrandList;
        public RepositoryBrand() 
        {
            BrandList = new List<Brand>();
        }
        public async Task<Brand> GetItemAsync(string id)
        {
            return await Task.FromResult(BrandList.FirstOrDefault(s => s.Oid.ToString() == id));
        }

        public async Task<List<Brand>> GetItemsAsync(bool forceRefresh = false)
        {
            if (!BrandList.Any())
                await GetItemsFromDBAsync();

            return await Task.FromResult(BrandList);
        }

        public async Task<List<Brand>> GetItemsFromDBAsync()
        {
            return await Task.Run(() =>
            {
                string queryString = $@"SELECT Oid
                                        ,Κωδικός
                                        ,Περιγραφή
                                        FROM ΙεραρχικόΖοομ1
                                        where GCRecord is null";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    BrandList = new List<Brand>();
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        BrandList.Add(new Brand()
                        {
                            Oid = Guid.Parse(reader["Oid"].ToString()),
                            BrandCode = reader["Κωδικός"] != DBNull.Value ? reader["Κωδικός"].ToString() : string.Empty,
                            Description = reader["Περιγραφή"] != DBNull.Value ? reader["Περιγραφή"].ToString() : string.Empty,
                        });
                    }
                    return BrandList;

                }
            });

        }

        public Task<bool> AddItemAsync(Brand item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(Brand item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemFromDBAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Brand>> GetItemsWithNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UploadItemAsync(Brand item)
        {
            throw new NotImplementedException();
        }
    }
}
