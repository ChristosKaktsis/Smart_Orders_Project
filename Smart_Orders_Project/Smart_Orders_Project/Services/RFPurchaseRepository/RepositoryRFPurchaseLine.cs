using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using SmartMobileWMS.Models;

namespace SmartMobileWMS.Services
{
    public class RepositoryRFPurchaseLine : RepositoryService
    {
        private List<RFPurchaseLine> RFPurchaseLines;
        private RFPurchaseLine rFPurchaseLine;
        int result = 0;
        public RepositoryRFPurchaseLine()
        {
            RFPurchaseLines = new List<RFPurchaseLine>();
        }

        public async Task<List<RFPurchaseLine>> GetItemsAsync(string id)
        {
            //get items from db 
            RFPurchaseLines.Clear();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("getPurchaseLines"), id);
            string queryString = sb.ToString();
//            string queryString = $@"SELECT Oid,RFΠωλήσεις,Είδος,Ποσότητα,BarCodeΕίδους,ΠοσότηταΔιάστασης,Μήκος,Πλάτος,Υψος
//FROM RFΓραμμέςΑγορών where RFΠωλήσεις = '{id}' and GCRecord is null";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    RFPurchaseLines.Add(new RFPurchaseLine
                    {
                        Oid = Guid.Parse(reader["Oid"].ToString()),
                        Product = await GetProduct(reader["Είδος"].ToString(), reader["BarCodeΕίδους"].ToString()),
                        ProductBarCode = reader["BarCodeΕίδους"].ToString(),
                        Quantity = decimal.Parse(reader["Ποσότητα"].ToString()),
                        RFSalesOid = Guid.Parse(reader["RFΠωλήσεις"].ToString()),
                        Width = reader["Πλάτος"] != DBNull.Value ? decimal.Parse(reader["Πλάτος"].ToString()) : 0,
                        Length = reader["Μήκος"] != DBNull.Value ? decimal.Parse(reader["Μήκος"].ToString()) : 0,
                        Height = reader["Υψος"] != DBNull.Value ? decimal.Parse(reader["Υψος"].ToString()) : 0,
                    });
                }
               
            }
            return await Task.FromResult(RFPurchaseLines);
        }
        private async Task<Product> GetProduct(string id, string bar)
        {
            return await Task.Run(async () =>
            {
                var lip = (string.IsNullOrEmpty(bar) ? "is null" : $"='{bar}'");

                string oldbe = $@"select * from (SELECT  Είδος.Oid     
                                      ,Κωδικός
                                      ,Περιγραφή
	                                  ,BarCode = null
                                      ,ΦΠΑ 
                                      ,ΜονάδεςΜέτρησης.ΜονάδαΜέτρησης
	                                  ,ΜονάδεςΜέτρησης.ΤύποςΔιάστασης
                                      ,Πλάτος
                                      ,Μήκος
                                      ,Υψος
                                      ,Κείμενο2 as ProductCode2
	                                  ,ΤιμήΧονδρικής
	                                  ,Χρώματα = null 
	                                  ,Μεγέθη = null
                                  FROM Είδος left join ΜονάδεςΜέτρησης on ΜονάδεςΜέτρησης.Oid = Είδος.ΜονάδαΜέτρησης
                                  where  Είδος.GCRecord is null

                                  UNION

                                  SELECT Είδος as Oid
	                                  ,BarCode as Κωδικός
                                      ,Περιγραφή
	                                  ,BarCode
	                                  ,ΦΠΑ
	                                  ,ΜονάδεςΜέτρησης.ΜονάδαΜέτρησης
	                                  ,ΜονάδεςΜέτρησης.ΤύποςΔιάστασης
	                                  ,Πλάτος
                                      ,Μήκος
                                      ,Υψος
	                                  ,Κείμενο2 as ProductCode2
                                      ,ΤιμήΧονδρικής
                                      ,Χρώματα.Χρώματα
                                      ,Μεγέθη.Μεγέθη
                                  FROM BarCodeΕίδους
                                  left join Χρώματα on Χρώματα.Oid = BarCodeΕίδους.Χρώμα
                                  left join ΜονάδεςΜέτρησης on ΜονάδεςΜέτρησης.Oid = BarCodeΕίδους.ΜονάδαΜέτρησης
                                  left join Μεγέθη on Μεγέθη.Oid = BarCodeΕίδους.Μέγεθος
                                   where BarCodeΕίδους.GCRecord is null) as U where U.Oid = '{id}' and U.BarCode {lip}";
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(await GetParamAsync("getProductWithID_Barcode"), id, lip);
                string queryString = sb.ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                        return null;
                    reader.Read();

                    Product selectProduct = new Product()
                    {
                        Oid = Guid.Parse(reader["Oid"].ToString()),
                        ProductCode = reader["Κωδικός"] != null ? reader["Κωδικός"].ToString() : string.Empty,
                        BarCode = reader["BarCode"] != DBNull.Value ? reader["BarCode"].ToString() : string.Empty,
                        Name = reader["Περιγραφή"].ToString(),
                        FPA = int.Parse(reader["ΦΠΑ"] != null ? reader["ΦΠΑ"].ToString() : "0"),
                        Price = double.Parse(reader["ΤιμήΧονδρικής"].ToString()),
                        LastPriceSold = reader["LastPrice"] != DBNull.Value ? double.Parse(reader["LastPrice"].ToString()) : 0,
                        Color = reader["Χρώματα"] != null ? reader["Χρώματα"].ToString() : string.Empty,
                        Size = reader["Μεγέθη"] != null ? reader["Μεγέθη"].ToString() : string.Empty,
                        Width = reader["Πλάτος"] != DBNull.Value ? float.Parse(reader["Πλάτος"].ToString()) : 0,
                        Length = reader["Μήκος"] != DBNull.Value ? float.Parse(reader["Μήκος"].ToString()) : 0,
                        Height = reader["Υψος"] != DBNull.Value ? float.Parse(reader["Υψος"].ToString()) : 0,
                        Type = int.Parse(reader["ΤύποςΔιάστασης"] != DBNull.Value ? reader["ΤύποςΔιάστασης"].ToString() : "0"),
                        UnitOfMeasure = string.IsNullOrEmpty(reader["ΜονάδαΜέτρησης"].ToString()) ? "Ποσότητα" : reader["ΜονάδαΜέτρησης"].ToString(),
                    };

                    return selectProduct;
                }
            });

        }
        public async Task<RFPurchaseLine> GetItemAsync(string id)
        {
            //get item from DB
            
            return await Task.FromResult(rFPurchaseLine);
        }
        public async Task<bool> AddItemAsync(RFPurchaseLine item)
        {
            //add item to DB
            string q = item.Quantity.ToString().Replace(',', '.');
            string h = item.Height.ToString().Replace(',', '.');
            string l = item.Length.ToString().Replace(',', '.');
            string w = item.Width.ToString().Replace(',', '.');
            var lip = string.IsNullOrEmpty(item.Product.BarCode) ? "null" : "'" + item.Product.BarCode + "'";


 //           string queryString = $@"INSERT INTO RFΓραμμέςΑγορών (Oid, RFΠωλήσεις, Είδος, Ποσότητα, BarCodeΕίδους, ΠοσότηταΔιάστασης, Μήκος, Πλάτος, Υψος)
 //VALUES((Convert(uniqueidentifier, N'{ item.Oid }')),(Convert(uniqueidentifier, N'{ item.RFSalesOid }')),(Convert(uniqueidentifier, N'{ item.Product.Oid }')), Convert(float,'{q}'), { lip }, Convert(float,'{ q }'), Convert(float,'{ l }'), Convert(float,'{ w }'), Convert(float,'{ h }')); ";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(await GetParamAsync("postPurchaseLine"), item.Oid, item.RFSalesOid, item.Product.Oid, q, lip, q, l, w, h);
            string queryString = sb.ToString();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                result = await command.ExecuteNonQueryAsync();
            }
            return await Task.FromResult(result != 0);
        }

        public async Task<bool> UpdateItemAsync(RFPurchaseLine item)
        {
            //update item from db

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemsAsync(string id)
        {
            //delete item from db
            //string queryDelete = $"DELETE From RFΓραμμέςΑγορών where RFΠωλήσεις = '{id}'";

            StringBuilder sb2 = new StringBuilder();
            sb2.AppendFormat(await GetParamAsync("deletePurchaseLine"), id);
            string queryDelete = sb2.ToString();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryDelete, connection);
                result = await command.ExecuteNonQueryAsync();
            }
            
            return await Task.FromResult(result!=0);
        }
    }
}
