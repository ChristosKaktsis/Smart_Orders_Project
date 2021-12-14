using Smart_Orders_Project.Models.SparePartModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    public class RepositoryManufacturer : RepositoryService, IDataStore<Manufacturer>
    {
        List<Manufacturer> ManufacturerList;
        public RepositoryManufacturer()
        {
            ManufacturerList = new List<Manufacturer>();
        }

        public async Task<bool> AddItemAsync(Manufacturer item)
        {
            ManufacturerList.Add(item);
            var s = await AddToDB(item);
            return await Task.FromResult(s);
        }

        private async Task<bool> AddToDB(Manufacturer item)
        {
            int ok = 0;
            string queryString = $@"INSERT INTO [Κατασκευαστής] (Oid, Κωδικός, Περιγραφή)
                                    VALUES((Convert(uniqueidentifier, N'{item.Oid}')), 
                                           '{item.ManufacturerCode}','{item.Description}')";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                ok = command.ExecuteNonQuery();
            }
            return await Task.FromResult(ok!=0);
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemFromDBAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Manufacturer> GetItemAsync(string id)
        {
            return await Task.FromResult(ManufacturerList.FirstOrDefault(s => s.Oid.ToString() == id));
        }

        public async Task<List<Manufacturer>> GetItemsAsync(bool forceRefresh = false)
        {
            if (!ManufacturerList.Any())
                await GetItemsFromDBAsync();

            return await Task.FromResult(ManufacturerList);
        }

        private async Task<List<Manufacturer>> GetItemsFromDBAsync()
        {
            return await Task.Run(() =>
            {
                string queryString = $@"Select Oid
                                       ,Κωδικός
                                       ,Περιγραφή
                                       FROM Κατασκευαστής
                                       where GCRecord is null";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    ManufacturerList = new List<Manufacturer>();
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ManufacturerList.Add(new Manufacturer()
                        {
                            Oid = Guid.Parse(reader["Oid"].ToString()),
                            ManufacturerCode = reader["Κωδικός"] != DBNull.Value ? reader["Κωδικός"].ToString() : string.Empty,
                            Description = reader["Περιγραφή"] != DBNull.Value ? reader["Περιγραφή"].ToString() : string.Empty,
                        });
                    }
                    return ManufacturerList;

                }
            });
        }

        public Task<List<Manufacturer>> GetItemsWithNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(Manufacturer item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UploadItemAsync(Manufacturer item)
        {
            throw new NotImplementedException();
        }
    }
}
