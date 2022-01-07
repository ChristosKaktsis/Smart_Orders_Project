using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    public class RepositoryBarCodeCounter : RepositoryService
    {
        public string Counter { get; set; }
        public int Value { get; set; }
        public async Task<int> GetCounterFromDB()
        {
            return await Task.Run(() =>
            {
                string queryString = "SELECT Μετρητής ,Τιμή FROM ΓενικόςΜετρητής where Μετρητής = 'Barcode' and GCRecord is null";

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
            return await Task.Run(() =>
            {
                string queryString = $"UPDATE ΓενικόςΜετρητής SET Τιμή = {Value} WHERE Μετρητής = 'Barcode'";

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
