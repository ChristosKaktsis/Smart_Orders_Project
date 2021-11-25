using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Smart_Orders_Project.Services
{
    public class RepositoryRFPosition : IDataStore<Position>
    {
        private string ConnectionString
        {
            get => Preferences.Get(nameof(ConnectionString), @"User Id=sa;password=1;Pooling=false;Data Source=192.168.3.44\SQLEXPRESS;Initial Catalog=maindemo");
        }
        public List<Position> PositionList;
        public RepositoryRFPosition()
        {
            PositionList = new List<Position>();
        }
        public Task<bool> AddItemAsync(Position item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Position> GetItemAsync(string id)
        {
            return await Task.Run(() => {
                
                string queryString = $"SELECT  Oid, Κωδικός, Περιγραφή FROM Θέση where (Κωδικός = '{id}' or Περιγραφή = '{id}') and GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                        return null;
                    reader.Read();
                    Position position = new Position
                    {
                        Oid = Guid.Parse(reader["Oid"].ToString()),
                        PositionCode = reader["Κωδικός"] == DBNull.Value ? "-Κενή-" : reader["Κωδικός"].ToString(),
                        Description = reader["Περιγραφή"] == DBNull.Value ? "-Κενή-" : reader["Περιγραφή"].ToString()
                    };
                    return position;
                }
            });
        }

        public  Task<List<Position>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Position>> GetItemsWithNameAsync(string name)
        {
            return await Task.Run(() => {
                PositionList.Clear();
                string queryString = "SELECT  Oid, Κωδικός, Περιγραφή FROM Θέση where GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        PositionList.Add(new Position
                        {
                            Oid = Guid.Parse(reader["Oid"].ToString()),
                            PositionCode = reader["Κωδικός"] == DBNull.Value ? "-Κενή-" : reader["Κωδικός"].ToString(),
                            Description = reader["Περιγραφή"] == DBNull.Value ? "-Κενή-" : reader["Περιγραφή"].ToString()
                        });
                    }
                    return PositionList;
                }
            });
        }

        public Task<bool> UpdateItemAsync(Position item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UploadItemAsync(Position item)
        {
            throw new NotImplementedException();
        }
    }
}
