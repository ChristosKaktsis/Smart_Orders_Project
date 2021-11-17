using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Smart_Orders_Project.Services
{
    public class RepositoryItems : IDataStore<Product>
    {
        private string ConnectionString
        {
            get => Preferences.Get(nameof(ConnectionString), @"User Id=sa;password=1;Pooling=false;Data Source=192.168.3.44\SQLEXPRESS;Initial Catalog=maindemo");
        }
        public RepositoryItems()
        {
            ProductsList = new List<Product>();
            //GetItemsFromDB();
        }

        private void GetItemsFromDB()
        {
            //string queryString = "select Oid , Κωδικός ,Περιγραφή ,ΦΠΑ ,ΤιμήΧονδρικής from Είδος where GCRecord is null";
            string queryString = @"SELECT  BarCode
                                  , BarCodeΕίδους.Περιγραφή as BarCodeDesc
                                  ,Είδος.Oid
	                              ,Είδος.Περιγραφή
	                              ,Είδος.ΤιμήΧονδρικής
	                              ,Είδος.Κωδικός
	                              ,Είδος.ΦΠΑ
	                              ,Χρώματα.Χρώματα
	                              ,Μεγέθη.Μεγέθη
	                              ,ΜονάδεςΜέτρησης.ΜονάδαΜέτρησης
                              FROM BarCodeΕίδους
                              right join Είδος on BarCodeΕίδους.Είδος = Είδος.Oid
                              left join Χρώματα on Χρώματα.Oid = BarCodeΕίδους.Χρώμα
                              left join ΜονάδεςΜέτρησης on ΜονάδεςΜέτρησης.Oid = BarCodeΕίδους.ΜονάδαΜέτρησης
                              left join Μεγέθη on Μεγέθη.Oid = BarCodeΕίδους.Μέγεθος";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ProductsList.Add(new Product
                    {
                        Oid = Guid.Parse(reader["Oid"].ToString()),
                        ProductCode = reader["Κωδικός"] != null ? reader["Κωδικός"].ToString() : string.Empty,
                        Name = reader["Περιγραφή"].ToString(),
                        FPA = int.Parse(reader["ΦΠΑ"] != null ? reader["ΦΠΑ"].ToString() : "0"),
                        Price = double.Parse(reader["ΤιμήΧονδρικής"].ToString()),
                        BarCode = reader["BarCode"].ToString(),
                        BarCodeDesc = reader["BarCodeDesc"].ToString(),
                        Color = reader["Χρώματα"] != null ? reader["Χρώματα"].ToString() : string.Empty,
                        Size = reader["Μεγέθη"] != null ? reader["Μεγέθη"].ToString() : string.Empty,
                        UnitOfMeasure = string.IsNullOrEmpty(reader["ΜονάδαΜέτρησης"].ToString()) ? "Ποσότητα" : reader["ΜονάδαΜέτρησης"].ToString() ,
                    });
                }
                //return ProductsList;
            }
        }

        public List<Product> ProductsList { get; set; }

        public Task<bool> AddItemAsync(Product item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetItemAsync(string id)
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
                                 
                              where Είδος.Κωδικός = '" + id+ "' OR BarCode = '" + id + "'";

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
                        Width = reader["Πλάτος"] != DBNull.Value ? double.Parse(reader["Πλάτος"].ToString()) : 0.0,
                        Length = reader["Μήκος"] != DBNull.Value ? double.Parse(reader["Μήκος"].ToString()) : 0.0,
                        Height = reader["Υψος"] != DBNull.Value ? double.Parse(reader["Υψος"].ToString()) : 0.0,
                        UnitOfMeasure = string.IsNullOrEmpty(reader["ΜονάδαΜέτρησης"].ToString()) ? "Ποσότητα" : reader["ΜονάδαΜέτρησης"].ToString(),
                    };

                    return selectProduct;
                }
            });
           
        }

        public async Task<List<Product>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(ProductsList);
        }

        public Task<bool> UpdateItemAsync(Product item)
        {
            throw new NotImplementedException();
        }
        

        public Task<bool> UploadItemAsync(Product item)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> GetItemsWithNameAsync(string name)
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
                                 
                              where Είδος.Κωδικός LIKE '%" + name + "%' OR BarCode LIKE '%" + name + "%' OR  Είδος.Περιγραφή LIKE '%" + name + "%'";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    /*ProductsList.Clear()*/
                    ProductsList = new List<Product>();
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    
                    
                    while (reader.Read())
                    {
                        ProductsList.Add(new Product()
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
                            Width = reader["Πλάτος"] != DBNull.Value ? double.Parse(reader["Πλάτος"].ToString()) : 0.0,
                            Length = reader["Μήκος"] != DBNull.Value ? double.Parse(reader["Μήκος"].ToString()) : 0.0,
                            Height = reader["Υψος"] != DBNull.Value ? double.Parse(reader["Υψος"].ToString()) : 0.0,
                            UnitOfMeasure = string.IsNullOrEmpty(reader["ΜονάδαΜέτρησης"].ToString()) ? "Ποσότητα" : reader["ΜονάδαΜέτρησης"].ToString(),
                        });
                    }
                    return ProductsList;

                }
            });

        }
    }
}
