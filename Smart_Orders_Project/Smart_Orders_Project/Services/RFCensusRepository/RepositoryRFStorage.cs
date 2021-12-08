using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Smart_Orders_Project.Services
{
    public class RepositoryRFStorage : RepositoryService, IDataStore<Storage>
    {
        
        public List<Storage> StorageList { get; set; }
        public RepositoryRFStorage()
        {
            StorageList = new List<Storage>();
        }
        public async Task<bool> AddItemAsync(Storage item)
        {
            StorageList.Add(item);

            return await Task.FromResult(true);
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Storage> GetItemAsync(string id)
        {
            return await Task.FromResult(StorageList.FirstOrDefault(s => s.Oid.ToString() == id));
        }

        public async Task<List<Storage>> GetItemsAsync(bool forceRefresh = false)
        {
           return await Task.Run(() => {
                StorageList.Clear();
                string queryString = "SELECT Oid, Περιγραφή FROM ΑποθηκευτικόςΧώρος where GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        StorageList.Add(new Storage
                        {
                            Oid = Guid.Parse(reader["Oid"].ToString()),
                            Description = reader["Περιγραφή"] == DBNull.Value ? "-Κενή-" : reader["Περιγραφή"].ToString()
                        });
                    }
                    return StorageList;
                }
            });
        }

        public Task<List<Storage>> GetItemsWithNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(Storage item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UploadItemAsync(Storage item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemFromDBAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
