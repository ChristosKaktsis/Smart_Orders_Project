using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Smart_Orders_Project.Services
{
    public class RepositoryRecievers : IDataStore<Reciever>
    {
        public List<Reciever> RecieverList { get; set; }
        private string ConnectionString
        {
            get => Preferences.Get(nameof(ConnectionString), @"User Id=sa;password=1;Pooling=false;Data Source=192.168.3.44\SQLEXPRESS;Initial Catalog=maindemo");
        }
        public Task<bool> AddItemAsync(Reciever item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Reciever> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Reciever>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.Run(() =>
            {
                RecieverList = new List<Reciever>();
                string queryString = @"select Oid ,Επωνυμία 
                                      from Παραλαβών where GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        RecieverList.Add(new Reciever
                        {
                            Oid = Guid.Parse(reader["Oid"].ToString()),
                            RecieverName = reader["Επωνυμία"] != DBNull.Value ? reader["Επωνυμία"].ToString() : string.Empty
                        });
                    }
                    return RecieverList;
                }
            });
        }

        public Task<List<Reciever>> GetItemsWithNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(Reciever item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UploadItemAsync(Reciever item)
        {
            throw new NotImplementedException();
        }
    }
}
