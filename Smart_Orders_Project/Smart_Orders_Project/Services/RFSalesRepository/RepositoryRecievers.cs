using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartMobileWMS.Services
{
    public class RepositoryRecievers : RepositoryService, IDataStore<Reciever>
    {
        public List<Reciever> RecieverList { get; set; }
        
        public Task<bool> AddItemAsync(Reciever item)
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

        public Task<Reciever> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Reciever>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.Run(async() =>
            {
                RecieverList = new List<Reciever>();
        
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(await GetParamAsync("getRecievers"));
                string queryString = sb.ToString();
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

        public async Task<bool> UploadItemAsync(Reciever item)
        {
            return await Task.Run(async() =>
            {
                int ok = 0;
                string oldqueryString = $@"INSERT INTO [Παραλαβών] (Oid, Επωνυμία)
                                    VALUES((Convert(uniqueidentifier, N'{item.Oid}')), 
                                           '{item.RecieverName}')";
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(await GetParamAsync("postReciever"), item.Oid, item.RecieverName);
                string queryString = sb.ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    ok = command.ExecuteNonQuery();
                }
                return ok >= 1 ? true : false;
            });
        }
    }
}
