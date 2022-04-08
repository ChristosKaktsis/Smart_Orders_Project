using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Smart_Orders_Project.Services
{
    public class RepositoryService
    {
        public string ConnectionString
        {
            get => Preferences.Get(nameof(ConnectionString), @"User ID=sa;Password=1;Pooling=false;Data Source=192.168.1.187,1433\SQLEXPRESS2019;Initial Catalog=SmartLobSidall");
            set
            {
                Preferences.Set(nameof(ConnectionString), value);
            }
        }
        protected async Task<string> GetParamAsync(string parName)
        {
            string queryString = $@"SELECT ParamName, Parameter
                From XamarinMobWMSParameters where ParamName='{parName}'";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                    return null;
                await reader.ReadAsync();

                return await Task.FromResult(reader["Parameter"].ToString());
            }

        }
    }
}
