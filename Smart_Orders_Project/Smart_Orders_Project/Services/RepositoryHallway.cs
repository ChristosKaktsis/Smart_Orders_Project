using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    public class RepositoryHallway : RepositoryService
    {
        public async Task<bool> SetAAToZero()
        {
            int result = 0;
           
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("setHallAAToZero"));
            string queryString = sb.ToString();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                result = await command.ExecuteNonQueryAsync();
            }
            return await Task.FromResult(result != 0);
        }
        public async Task<Hallway> GetHallWay(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getHallWay"), id);
            string queryString = sb.ToString();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                    return null;
                await reader.ReadAsync();
                Hallway position = new Hallway
                {
                    Oid = Guid.Parse(reader["Oid"].ToString()),
                    Code = reader["Κωδικός"] == DBNull.Value ? "-Κενή-" : reader["Κωδικός"].ToString(),
                    Description = reader["Περιγραφή"] == DBNull.Value ? "-Κενή-" : reader["Περιγραφή"].ToString(),
                };
                return await Task.FromResult(position);
            }
        }
        public async Task<bool> UpdateHallWay(Hallway hallway)
        {
            int result = 0;
            if (hallway == null)
                return await Task.FromResult(false);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("putHallWay"), hallway.Code, hallway.AAPicking);
            string queryString = sb.ToString();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                result = await command.ExecuteNonQueryAsync();
            }
            return await Task.FromResult(result != 0);
        }
    }
}
