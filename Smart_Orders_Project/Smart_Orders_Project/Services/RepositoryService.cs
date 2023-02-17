using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartMobileWMS.Services
{
    public class RepositoryService
    {
        public string ConnectionString
        {
            get => Preferences.Get(nameof(ConnectionString), App.Current.Resources["ConnectionString"] as string);
            set
            {
                Preferences.Set(nameof(ConnectionString), value);
            }
        }
        protected async Task<string> GetParamAsync(string parName)
        {
            return await Task.Run( async() => {
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
            });
        }
    }
}
