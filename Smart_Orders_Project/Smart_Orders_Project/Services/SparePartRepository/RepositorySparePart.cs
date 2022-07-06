using SmartMobileWMS.Models.SparePartModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.Services
{
    public class RepositorySparePart : RepositoryService
    {
        public RepositorySparePart()
        {

        }
        public async void SendJSON(SparePart item)
        {
            await Task.Run( async () =>
            {
                string json =  SetUpJSON(item);
                if (string.IsNullOrEmpty(json))
                {
                    await Shell.Current.DisplayAlert("Σφάλμα!", "Το είδος Δεν αποθηκεύτηκε.", "Οκ");
                    return;
                }
                    
                string queryString = @"INSERT INTO ExternalXml (Oid,XmlData,ΗμνίαΔημιουργίας)
                    VALUES(NEWID(),'" + json + "',(Convert(date, '" + DateTime.Now.ToString("MM/dd/yyyy") + "')))";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.ExecuteNonQuery();
                }

            });
        }

        private string SetUpJSON(SparePart item)
        {
            try
            {
                if (item == null)
                    return string.Empty;
                string json = string.Empty;
                var groupingId = item.Grouping != null ? item.Grouping.ID : "";
                var brandcode = item.Brand != null ? item.Brand.BrandCode : "";
                var modelcode = item.Model != null ? item.Model.ModelCode : "";
                var manufacturer = item.Manufacturer != null ? item.Manufacturer.Oid.ToString() : "";
                json = "{\"Κωδικός\":\"" + item.SparePartCode + "\",";
                json += "\"Περιγραφή\":\"" + item.Description + "\",";
                json += "\"Δενδρική Ομαδοποίηση Ειδών\":\"" + groupingId + "\",";
                json += "\"Ιεραρχικό Ζοομ 1\":\"" + brandcode + "\",";
                json += "\"Ιεραρχικό Ζοομ 2\":\"" + modelcode + "\",";
                json += "\"Ετος Από\":\"" + item.YearFrom + "\",";
                json += "\"Ετος Εως\":\"" + item.YearTo + "\",";
                json += "\"Κωδικος Κατασκευαστη\":\"" + item.ManufacturerCode + "\",";
                json += "\"Κωδικος After Market\":\"" + item.AfterMarketCode + "\",";
                json += "\"Κατασκευαστης Ειδους\":\"" + manufacturer + "\",";
                json += "\"Κατασταση\":\"" + item.Condition + "\",";
                json += "\"Τιμή Χονδρικής\":\"" + item.PriceWholesale + "\",";
                json += "\"Τιμή Λιανικής\":\"" + item.PriceRetail + "\"";
                json += "\"Image\":\"" + Convert.ToBase64String(item.ImageBytes) + "\"";
                json += "}";
                return json;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                return string.Empty;
            }
            
        }
    }
}
