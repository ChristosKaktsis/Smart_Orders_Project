using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    public class RepositoryPalette : RepositoryService
    {
        int result = 0;
        public async Task<Palette> GetPalette(string sscc)
        {
            
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getPalette"), sscc);
            string queryString = sb.ToString();
            
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                    return null;
                await reader.ReadAsync();
                Palette palette = new Palette()
                {
                    Oid = Guid.Parse(reader["Oid"].ToString()),
                    SSCC = reader["SSCC"] != DBNull.Value ? reader["SSCC"].ToString() : string.Empty,
                    Description = reader["Περιγραφή"] != DBNull.Value ? reader["Περιγραφή"].ToString() : string.Empty,
                    Height = float.Parse(reader["Υψος"].ToString()),
                    Width = float.Parse(reader["Πλάτος"].ToString()),
                    Length = float.Parse(reader["Μήκος"].ToString()),
                };
                return await Task.FromResult(palette);
            }
        }
        public async Task<bool> PostPalette(Palette palette)
        {
            if (palette == null)
                return await Task.FromResult(false);
 
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("postPalette"),palette.Oid,palette.SSCC,palette.Description,palette.Length,palette.Width,palette.Height);
            string queryString = sb.ToString();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                result = await command.ExecuteNonQueryAsync();
            }
            return await Task.FromResult(result != 0);
        }
        public async Task<bool> PostPaletteItem(Palette palette,Product product)
        {
            if (palette == null || product == null)
                return await Task.FromResult(false);
            var oid = Guid.NewGuid();
            var barcode = string.IsNullOrEmpty(product.BarCode) ? "null" : product.BarCode;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("postPaletteItem"),oid, palette.Oid, product.Oid, barcode, product.Quantity);
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
