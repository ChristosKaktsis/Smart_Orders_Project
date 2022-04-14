using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Smart_Orders_Project.Services
{
    public static class RepositoryPrint 
    {
        public  static async Task<List<string>> GetPrinters()
        {
            return await Task.Run(() =>
            {
                List<string> printers = new List<string>();
                string ConnectionString = Preferences.Get("ConnectionString", App.Current.Resources["ConnectionString"] as string);
                string queryString = "select Περιγραφή  " +
                "from ΠίνακαςΔεδομένων15 where GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        printers.Add(reader["Περιγραφή"].ToString());
                    }
                    return printers;
                }
            });
        }
        public async static void SendPrint(string printer , string RF)
        {
            await Task.Run(() => 
            {
                string ConnectionString = Preferences.Get("ConnectionString", @"User Id=sa;password=1;Pooling=false;Data Source=192.168.3.44\SQLEXPRESS;Initial Catalog=maindemo");
                string json="{\"Εκτυποτής\":\""+printer+"\",\"RFΠώληση\":\""+RF+"\"}";
                
                string queryString = @"INSERT INTO ExternalXml (Oid,XmlData,ΗμνίαΔημιουργίας)
                    VALUES(NEWID(),'"+ json + "',(Convert(date, '" + DateTime.Now.ToString("MM/dd/yyyy") + "')))";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.ExecuteNonQuery();
                }
                
            });
        }
    }
}
