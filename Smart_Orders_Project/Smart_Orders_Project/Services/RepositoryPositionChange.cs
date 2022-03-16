using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    public class RepositoryPositionChange: RepositoryService
    {
        int result = 0;
        public async Task<bool> PositionChange(Position position, Product product, int quantity, int impexp)
        {
            if (position == null || product == null)
                return await Task.FromResult(false);
            var Oid = Guid.NewGuid();
            var barcode = string.IsNullOrEmpty(product.BarCode) ? "null" : product.BarCode;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("postPosition"),Oid, position.Oid, barcode, product.Oid, quantity, impexp);
            string queryString = sb.ToString();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                result = command.ExecuteNonQuery();
            }
            return await Task.FromResult(result != 0);
        }
    }
    
}
