using SmartMobileWMS.Constants;
using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    public class BaseRepository
    {
        protected async Task<string> GetParamAsync(string parName)
        {
            return await Task.Run(async () => {
                string queryString = $@"SELECT ParamName, Parameter
                From XamarinMobWMSParameters where ParamName='{parName}'";
                using (SqlConnection connection = new SqlConnection(ConnectionStrings.ConnectionString))
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
        /// <summary>
        /// Executes a query get method and returns JSON result 
        /// </summary>
        /// <param name="method">method query whitch can get by calling GetParamAsync and append any parm</param>
        /// <returns></returns>
        protected async Task<string> ExecuteGetMethod(string method)
        {
            var jsonResult = new StringBuilder();

            using (SqlConnection connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(method, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    jsonResult.Append(reader.GetValue(0).ToString());
                }
            }
            return jsonResult.ToString();
        }
        protected async Task<int> ExecutePostMethod(string method)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(method, connection);
                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}
