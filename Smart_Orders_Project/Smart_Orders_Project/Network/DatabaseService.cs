using SmartMobileWMS.Constants;
using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZXing;

namespace SmartMobileWMS.Network
{
    public static class DatabaseService
    {
        public static async Task<bool> ParametersExist()
        {
            var method = "IF( EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'XamarinMobWMSParameters')) SELECT CAST(1 as BIT) as result for json path,without_array_wrapper ELSE SELECT CAST(0 as BIT) as result for json path,without_array_wrapper";
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
            var result = JsonSerializer.Deserialize<ThisResult>(jsonResult.ToString());
            return result.result;
        }
        public static async Task<bool> ParameterExist(string name)
        {
            var method = $"IF( EXISTS (SELECT * FROM XamarinMobWMSParameters WHERE ParamName='{name}' and GCRecord is null)) SELECT CAST(1 as BIT) as result for json path,without_array_wrapper ELSE SELECT CAST(0 as BIT) as result for json path,without_array_wrapper";
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
            var result = JsonSerializer.Deserialize<ThisResult>(jsonResult.ToString());
            return result.result;
        }
        public static async Task<string> GetVersion()
        {
            var method = "SELECT SERVERPROPERTY('ProductVersion') AS ProductVersion";
            using (SqlConnection connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(method, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                reader.Read();
                var result = reader["ProductVersion"].ToString();
                return result.Substring(0, 2);
            }
        }
        public static async Task<int> NoOfParameters()
        {
            var method = "SELECT Count(*) as rows FROM XamarinMobWMSParameters where GCRecord is null for json path,without_array_wrapper";
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
            var result = JsonSerializer.Deserialize<ThisResult> (jsonResult.ToString());
            return result.rows;
        }
        class ThisResult
        {
            public bool result { get; set; }
            public int rows { get; set; }
        }
    }
}
