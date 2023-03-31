using SmartMobileWMS.Constants;
using SmartMobileWMS.Resources;
using System;
using System.Collections;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.Services
{
    internal static class DatabaseParameter
    {
        public static async Task CreateParameters()
        {
            try
            {
                ResourceSet resourceSet = Parameters
               .ResourceManager
               .GetResourceSet(CultureInfo.CurrentUICulture, true, true);
                foreach (DictionaryEntry entry in resourceSet)
                {
                    string resourceKey = entry.Key.ToString();
                    string resource = entry.Value.ToString();
                    resource = resource.Replace("'", "''");
                    await AddToDatabase(resourceKey, resource);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private static async Task AddToDatabase(string resourceKey, string resource)
        {
            var method = "INSERT INTO XamarinMobWMSParameters(Oid,ParamName,Parameter) Values(NEWID(),'{0}','{1}');";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(method, resourceKey,resource);
            using (SqlConnection connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sb.ToString(), connection);
                await command.ExecuteNonQueryAsync();
            }
        }
        public static async Task DeleteParameters()
        {
            var method = "DELETE From XamarinMobWMSParameters";
            
            using (SqlConnection connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(method, connection);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
