using SmartMobileWMS.Constants;
using SmartMobileWMS.Models;
using SmartMobileWMS.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.Repositories
{
    public class BaseRepository
    {
        protected async Task<string> GetParamAsync(string parName)
        {
            string queryString = $@"SELECT ParamName, Parameter
                From XamarinMobWMSParameters where ParamName='{parName}' and GCRecord is null";
            using (SqlConnection connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    NotifyUser(parName);
                    return string.Empty;
                }
                await reader.ReadAsync();
                if (reader["Parameter"] == string.Empty) 
                { 
                    NotifyUser(parName);
                    return string.Empty;
                } 
                return await Task.FromResult(reader["Parameter"].ToString());
            }
        }
        /// <summary>
        /// Executes a query get method and returns JSON result 
        /// </summary>
        /// <param name="method">method query whitch can get by calling GetParamAsync and append any parm</param>
        /// <returns></returns>
        protected async Task<string> ExecuteGetMethod(string method)
        {
            if (string.IsNullOrWhiteSpace(method)) return string.Empty;
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
            if (await ActivationService.UseExpired()) return 0;
            using (SqlConnection connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(method, connection);
                return await command.ExecuteNonQueryAsync();
            }
        }


        protected async void NotifyUser(string parameter)
        {
            await AppShell.Current.DisplayAlert(
                            "",
                            $"Δεν βρέθηκε η μέθοδος {parameter} ή είναι κενή.",
                            "OK");
        }
    }
}
