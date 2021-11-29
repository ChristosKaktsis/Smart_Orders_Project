using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Smart_Orders_Project.Services
{
   public class RepositoryUser : IUser<User>
    {
        User LoggedUser;

        public RepositoryUser()
        {
            
        }

        public async Task<User> GetUser()
        {
            return await Task.FromResult(LoggedUser);
        }
        public async Task<User> GetUserFromDB(string username, string password)
        {
            return await Task.Run(() =>
            {
                string queryString = $"select Oid, UserName from [User] where UserName ='{username}'";


                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                        return LoggedUser = null;
                    reader.Read();
                    
                    User user = new User()
                    {
                        UserID =  Guid.Parse(reader["Oid"].ToString()),
                        UserName = reader["UserName"] == DBNull.Value ? "null User" : reader["UserName"].ToString()
                    };
                    LoggedUser = user;
                    return user;
                }
            });
        }

       

        private string ConnectionString
        {
            get => Preferences.Get(nameof(ConnectionString), @"User Id=sa;password=1;Pooling=false;Data Source=192.168.3.44\SQLEXPRESS;Initial Catalog=maindemo");
        }
    }
}
