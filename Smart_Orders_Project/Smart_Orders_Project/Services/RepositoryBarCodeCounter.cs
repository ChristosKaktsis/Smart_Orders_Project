using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.Services
{
    public class RepositoryBarCodeCounter : RepositoryService
    {
        public string Counter { get; set; }
        public int Value { get; set; }
        public async Task<int> GetCounterFromDB()
        {
            return await Task.Run(async() =>
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(await GetParamAsync("getBarCodeCounter"));
                string queryString = sb.ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();

                    Counter = reader["Μετρητής"] == DBNull.Value ? "null" : reader["Μετρητής"].ToString();
                    Value = reader["Τιμή"] == DBNull.Value ? -1 : int.Parse(reader["Τιμή"].ToString()) + 1;

                    return Value;
                }
            });
           
        }
        public async Task<bool> SetCounterToDB()
        {
            return await Task.Run(async() =>
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(await GetParamAsync("putBarCodeCounter"),Value);
                string queryString = sb.ToString();

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    var ex = command.ExecuteNonQuery();
                    return ex != 0;
                }
            });
            
        }
    }
}
