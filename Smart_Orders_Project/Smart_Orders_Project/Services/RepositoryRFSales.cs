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
    class RepositoryRFSales : RepositoryService, IDataStore<RFSales>
    {
        public List<RFSales> RFSalesList;
        
        public RepositoryRFSales()
        {
            RFSalesList = new List<RFSales>();
        }

        public async Task<bool> AddItemAsync(RFSales item)
        {
            RFSalesList.Add(item);

            return await Task.FromResult(true);
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<RFSales> GetItemAsync(string id)
        {
            return await Task.FromResult(RFSalesList.FirstOrDefault(s => s.Oid == Guid.Parse(id)));
        }

        public async Task<List<RFSales>> GetItemsAsync(bool forceRefresh = false)
        {
            return await  Task.Run(async () =>
            {
                //RFSalesList = new List<RFSales>(); // αμα δεν τραβαμε τα δεδομενα 
                GetRFCounter();
                RFSalesList.Clear();
                string queryString = @"select Oid , Πελάτης ,ΠαραστατικόΠελάτη ,UpdSmart ,Ολοκληρώθηκε ,ΗμνίαΔημιουργίας ,Παραλαβών 
                                       from RFΠωλήσεις where UpdSmart = 'false' and GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        
                        RFSalesList.Add(new RFSales
                        {
                            Oid = Guid.Parse(reader["Oid"].ToString()),
                            RFCount = reader["ΠαραστατικόΠελάτη"].ToString(),
                            Customer =  await GetCustomer(reader["Πελάτης"].ToString()),
                            CreationDate = (DateTime)reader["ΗμνίαΔημιουργίας"],
                            Complete = bool.Parse(reader["Ολοκληρώθηκε"].ToString()),
                            Lines = await GetLines(reader["Oid"].ToString()),
                            Reciever = await GetReciever(reader["Παραλαβών"].ToString())
                        });
                    }
                    return RFSalesList;
                }
            });
        }

        private async Task<Reciever> GetReciever(string id)
        {
            return await Task.Run(() =>
            {
                string queryString = $@"select Oid ,Επωνυμία 
                                      from Παραλαβών where Oid = '{id}' and GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                        return null;
                    reader.Read();
                    Reciever reciever = new Reciever()
                    {
                        Oid = Guid.Parse(reader["Oid"].ToString()),
                        RecieverName = reader["Επωνυμία"] != DBNull.Value ? reader["Επωνυμία"].ToString() : string.Empty   
                    };

                    return reciever;
                }
            });
        }

        private void GetRFCounter()
        {
            string queryString = "SELECT Τιμή FROM ΓενικόςΜετρητής where Μετρητής='ΜετρητήςRF' and GCRecord is null";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    reader.Read();
                    Preferences.Set("RFCounter", int.Parse(reader["Τιμή"].ToString()));
                }    
            }
        }

        private async Task<List<LineOfOrder>> GetLines(string id)
        {
            return await Task.Run(async () =>
            {
                var LineList = new List<LineOfOrder>();
                string queryString = "select Oid, RFΠωλήσεις , Είδος ,Ποσότητα ,BarCodeΕίδους ,ΠοσότηταΔιάστασης ,Μήκος ,Πλάτος ,Υψος from RFΓραμμέςΠωλήσεων where RFΠωλήσεις ='" + id+"' and GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        LineList.Add(new LineOfOrder
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
                    return LineList;
                }
            });
        }

        private async Task<Product> GetProduct(string id , string bar)
        {
            return await Task.Run(() =>
            {
                string queryString = $@"select * from (SELECT  Είδος.Oid     
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
                                   where BarCodeΕίδους.GCRecord is null) as U where U.Oid = '{id}' and U.BarCode {(string.IsNullOrEmpty(bar)?"is null":$"='{bar}'")}";

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

        private async Task<Customer> GetCustomer(string Oid)
        {
            return await Task.Run(() =>
            {
                string queryString = "select Oid , Κωδικός ,Επωνυμία ,ΑΦΜ ,Email from Πελάτης where Oid='"+Oid+"' and GCRecord is null";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                        return null;
                    reader.Read();
                    Customer customer = new Customer()
                    {
                        Oid = Guid.Parse(reader["Oid"].ToString()),
                        CodeNumber = reader["Κωδικός"] != null ? reader["Κωδικός"].ToString() : string.Empty,
                        Name = reader["Επωνυμία"].ToString(),
                        AFM = reader["ΑΦΜ"].ToString(),
                        Email = reader["Email"].ToString()
                    };

                    return customer;
                }
            });
        }

        public async Task<bool> UpdateItemAsync(RFSales item)
        {
            return await Task.Run(() =>
            {
                int ok = 0;

                string queryString = $@"UPDATE RFΠωλήσεις
                                    SET Πελάτης = '{item.Customer.Oid}' ,
                                        Ολοκληρώθηκε = '{item.Complete}' , 
                                        UpdSmart = '{item.Complete}' ,
                                        Παραλαβών = {(item.Reciever == null ? "null" : $"'{item.Reciever.Oid}'")}
                                    WHERE Oid = '{item.Oid}' ";
                string queryDelete = $"DELETE From RFΓραμμέςΠωλήσεων where RFΠωλήσεις = '{item.Oid}'";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    if (!item.Complete)
                    {
                        SqlCommand command1 = new SqlCommand(queryDelete, connection);
                        var ex = command1.ExecuteNonQuery();
                    }
                    SqlCommand command = new SqlCommand(queryString, connection);
                    ok = command.ExecuteNonQuery();
                }
                return ok >= 1 ? true : false;
            });
        }
        public async Task<bool> UploadItemAsync(RFSales item)
        {
            return await Task.Run(() =>
            {
                int ok = 0;
                UpdateRFCounter();
                string queryString = $@"INSERT INTO RFΠωλήσεις (Oid, ΑποθηκευτικόςΧώρος, Πελάτης, ΠαραστατικάΠωλήσεων, ΠαραστατικόΠελάτη, 
                    Διαχείριση, UpdSmart, Ολοκληρώθηκε, ΗμνίαΔημιουργίας, ΑυτόματηΔιαγραφήΠαραστατικών, OptimisticLockField, GCRecord, Παραλαβών)
                    VALUES((Convert(uniqueidentifier, N'{item.Oid}')), null, (Convert(uniqueidentifier, N'{item.Customer.Oid}'))
                    , null, '{item.RFCount}', null, '0', '0', GETDATE(), '0', '1', null, 
                    {(item.Reciever==null?"null":$"'{item.Reciever.Oid}'")}); ";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    ok = command.ExecuteNonQuery();
                }
                return ok >= 1 ? true : false ;
            });
        }

        private void UpdateRFCounter()
        {
            string queryString = @"UPDATE ΓενικόςΜετρητής 
                                SET Τιμή = '"+ Preferences.Get("RFCounter", 1) + @"'
                                WHERE Μετρητής = 'ΜετρητήςRF'";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.ExecuteNonQuery();
            }
        }

        public Task<List<RFSales>> GetItemsWithNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemFromDBAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
