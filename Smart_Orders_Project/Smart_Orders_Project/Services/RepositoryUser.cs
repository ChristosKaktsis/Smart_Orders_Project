using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartMobileWMS.Services
{
   public class RepositoryUser : RepositoryService,IUser<User>
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
            return await Task.Run( async() =>
            {
                
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(await GetParamAsync("getUserWithID"), username);
                string queryString = sb.ToString();

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
                        UserID = Guid.Parse(reader["Oid"].ToString()),
                        UserName = reader["UserName"] == DBNull.Value ? "null User" : reader["UserName"].ToString()
                    };
                    LoggedUser = user;
                    return user;
                }
            });
        }
    }
}
