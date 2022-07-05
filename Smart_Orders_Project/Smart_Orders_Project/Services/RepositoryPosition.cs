using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Smart_Orders_Project.Models;

namespace Smart_Orders_Project.Services
{
    public class RepositoryPosition : RepositoryService
    {
        public async Task<bool> SetAAToZero()
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("setPositionAAToZero"));
            string queryString = sb.ToString();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                result = await command.ExecuteNonQueryAsync();
            }
            return await Task.FromResult(result != 0);

        }
        public async Task<Position> GetPosition(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getPositionWithID"), id);
            string queryString = sb.ToString();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                    return null;
                await reader.ReadAsync();
                Position position = new Position
                {
                    Oid = Guid.Parse(reader["Oid"].ToString()),
                    PositionCode = reader["Κωδικός"] == DBNull.Value ? "-Κενή-" : reader["Κωδικός"].ToString(),
                    Description = reader["Περιγραφή"] == DBNull.Value ? "-Κενή-" : reader["Περιγραφή"].ToString(),
                };
                return await Task.FromResult(position);
            }
        }
        public async Task<bool> UpdatePosition(Position position , Hallway hallway)
        {
            int result = 0;

            if (position == null || hallway == null)
                return await Task.FromResult(false);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("putPosition"),position.PositionCode,position.AAPicking,hallway.Oid);
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
