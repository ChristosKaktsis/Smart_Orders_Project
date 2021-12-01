using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Smart_Orders_Project.Services
{
    public class RepositoryRFCensus : RepositoryService, IDataStore<RFCensus>
    {
        
        List<RFCensus> RFCensusList;
        public RepositoryRFCensus()
        {
            RFCensusList = new List<RFCensus>();
        }
        public async Task<bool> AddItemAsync(RFCensus item)
        {
            RFCensusList.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = RFCensusList.Where((RFCensus arg) => arg.Oid.ToString() == id).FirstOrDefault();
            RFCensusList.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<RFCensus> GetItemAsync(string id)
        {
            return await Task.FromResult(RFCensusList.FirstOrDefault(s => s.Oid.ToString() == id));
        }

        public async Task<List<RFCensus>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(RFCensusList);
        }

        public async Task<List<RFCensus>> GetItemsWithNameAsync(string name)
        {
           return await Task.Run(async () =>
            {
                List<RFCensus> newlist = new List<RFCensus>();
                string queryString = $@"select Oid ,ΑποθηκευτικόςΧώρος ,Είδος ,Ποσότητα 
                                            ,ΗμνίαΔημιουργίας ,Θέση ,BarCodeΕίδους ,Ράφι 
                                       from RFΑπογραφή 
                                       where ΧρήστηςΔημιουργίας ='{name}' and UpdSmart = 'false' and UpdWMS = 'false'  and GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        newlist.Add(new RFCensus
                        {
                            Oid = Guid.Parse(reader["Oid"].ToString()),
                            CreationDate = reader["ΗμνίαΔημιουργίας"] == DBNull.Value ? DateTime.Now : DateTime.Parse(reader["ΗμνίαΔημιουργίας"].ToString()),
                            Quantity= reader["Ποσότητα"] == DBNull.Value ? 0 : decimal.Parse(reader["Ποσότητα"].ToString()),
                            Storage = await GetStorageDB(reader["ΑποθηκευτικόςΧώρος"].ToString()),
                            Position = await GetPositionDB(reader["Θέση"].ToString()),
                            Product = await GetProduct(reader["Είδος"].ToString(), reader["BarCodeΕίδους"].ToString()),
                        });
                    }
                    return newlist;
                }
            });
        }

        private async Task<Product> GetProduct(string id, string bar)
        {
            return await Task.Run(() =>
            {
                string queryString = @"SELECT  BarCode
                                  , BarCodeΕίδους.Περιγραφή as BarCodeDesc
                                  ,Είδος.Oid
	                              ,Είδος.Περιγραφή
	                              ,Είδος.ΤιμήΧονδρικής
	                              ,Είδος.Κωδικός
	                              ,Είδος.ΦΠΑ
                                  ,Είδος.Εκπτωση
	                              ,Χρώματα.Χρώματα
	                              ,Μεγέθη.Μεγέθη
                                  ,BarCodeΕίδους.Πλάτος
                                  ,BarCodeΕίδους.Μήκος
                                  ,BarCodeΕίδους.Υψος
	                              ,ΜονάδεςΜέτρησης.ΜονάδαΜέτρησης
                                  ,ΜονάδεςΜέτρησης.ΤύποςΔιάστασης
                              FROM BarCodeΕίδους
                              right join Είδος on BarCodeΕίδους.Είδος = Είδος.Oid
                              left join Χρώματα on Χρώματα.Oid = BarCodeΕίδους.Χρώμα
                              left join ΜονάδεςΜέτρησης on ΜονάδεςΜέτρησης.Oid = BarCodeΕίδους.ΜονάδαΜέτρησης
                              left join Μεγέθη on Μεγέθη.Oid = BarCodeΕίδους.Μέγεθος
                                 
                              where Είδος.Oid = '" + id + "' and BarCode " + (string.IsNullOrEmpty(bar) ? "is null" : "= '" + bar + "'");

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
                        Name = reader["Περιγραφή"].ToString(),
                        FPA = int.Parse(reader["ΦΠΑ"] != null ? reader["ΦΠΑ"].ToString() : "0"),
                        Price = double.Parse(reader["ΤιμήΧονδρικής"].ToString()),
                        Discount = int.Parse(reader["Εκπτωση"] != DBNull.Value ? reader["Εκπτωση"].ToString() : "0"),
                        BarCode = reader["BarCode"].ToString(),
                        BarCodeDesc = reader["BarCodeDesc"].ToString(),
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

        private async Task<Position> GetPositionDB(string id)
        {
            return await Task.Run(() => {

                string queryString = $"SELECT  Oid, Κωδικός, Περιγραφή FROM Θέση where Oid = '{id}' and GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                        return null;
                    reader.Read();
                    Position position = new Position
                    {
                        Oid = Guid.Parse(reader["Oid"].ToString()),
                        PositionCode = reader["Κωδικός"] == DBNull.Value ? "-Κενή-" : reader["Κωδικός"].ToString(),
                        Description = reader["Περιγραφή"] == DBNull.Value ? "-Κενή-" : reader["Περιγραφή"].ToString()
                    };
                    return position;
                }
            });
        }

        private async Task<Storage> GetStorageDB(string id)
        {
            return await Task.Run(() => {

                string queryString = $"SELECT Oid, Περιγραφή FROM ΑποθηκευτικόςΧώρος where Oid ='{id}' and GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                        return null;
                    reader.Read();
                    Storage storage = new Storage
                    {
                        Oid = Guid.Parse(reader["Oid"].ToString()),
                        Description = reader["Περιγραφή"] == DBNull.Value ? "-Κενή-" : reader["Περιγραφή"].ToString()
                    };
                    return storage;
                }
            });
        }

        public async Task<bool> UpdateItemAsync(RFCensus item)
        {
            return await Task.Run(() =>
            {
                int ok = 0;
                string queryString = $@"UPDATE RFΑπογραφή
                                    SET Ποσότητα = '{item.Quantity}'
                                    WHERE Oid = '{item.Oid}' ";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    ok = command.ExecuteNonQuery();
                }
                return ok >= 1 ? true : false;
            });
        }

        public async Task<bool> UploadItemAsync(RFCensus item)
        {
            return await Task.Run(() =>
            {
                int ok = 0;
                string queryString = $@"INSERT INTO RFΑπογραφή (Oid, ΑποθηκευτικόςΧώρος, Είδος, ΧρήστηςΔημιουργίας, Ποσότητα, ΗμνίαΔημιουργίας, 
                                    Θέση, BarCodeΕίδους ,UpdSmart ,UpdWMS)
                                    VALUES((Convert(uniqueidentifier, N'{item.Oid}')), 
                                           (Convert(uniqueidentifier, N'{item.Storage.Oid}')),
                                           (Convert(uniqueidentifier, N'{item.Product.Oid}')),
                                           (Convert(uniqueidentifier, N'{item.UserCreator.UserID}')),
                                        '{item.Quantity}', GETDATE(),(Convert(uniqueidentifier, N'{item.Position.Oid}')),
                                        "+(string.IsNullOrEmpty(item.Product.BarCode)?"null":$"'{item.Product.BarCode}'")+",0,0)";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    ok = command.ExecuteNonQuery();
                }
                return ok >= 1 ? true : false;
            });
        }

        public async Task<bool> DeleteItemFromDBAsync(string id)
        {
            var oldItem = RFCensusList.Where((RFCensus arg) => arg.Oid.ToString() == id).FirstOrDefault();
            RFCensusList.Remove(oldItem);

            return await Task.Run(() =>
            {
                int ok = 0;
                string queryString = $@"DELETE FROM RFΑπογραφή WHERE Oid ='{id}'";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    ok = command.ExecuteNonQuery();
                }
                return ok >= 1 ? true : false;
            });
        }
    }
}
