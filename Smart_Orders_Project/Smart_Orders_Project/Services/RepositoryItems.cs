using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartMobileWMS.Services
{
    public class RepositoryItems : RepositoryService, IDataStore<Product>
    {
        
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
            return await Task.Run(async() =>
            {
                
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(await GetParamAsync("getProductWithID"), id);
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
                        ProductCode = reader["Κωδικός"] != DBNull.Value ? reader["Κωδικός"].ToString() : string.Empty,
                        ProductCode2 = reader["ProductCode2"] != DBNull.Value ? reader["ProductCode2"].ToString() : string.Empty,
                        Name = reader["Περιγραφή"].ToString(),
                        BarCode = reader["BarCode"] != DBNull.Value ? reader["BarCode"].ToString() : string.Empty,
                        FPA = int.Parse(reader["ΦΠΑ"] != DBNull.Value ? reader["ΦΠΑ"].ToString() : "0"),
                        Price = reader["ΤιμήΧονδρικής"] != DBNull.Value ? double.Parse(reader["ΤιμήΧονδρικής"].ToString()) : 0,
                        LastPriceSold = reader["LastPrice"] != DBNull.Value ? double.Parse(reader["LastPrice"].ToString()) : 0,
                        Color = reader["Χρώματα"] != DBNull.Value ? reader["Χρώματα"].ToString() : string.Empty,
                        Size = reader["Μεγέθη"] != DBNull.Value ? reader["Μεγέθη"].ToString() : string.Empty,
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
            return await Task.Run(async() =>
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(await GetParamAsync("getProductWithName"), name);
                string queryString = sb.ToString();
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
                            ProductCode = reader["Κωδικός"] != DBNull.Value ? reader["Κωδικός"].ToString() : string.Empty,
                            ProductCode2 = reader["ProductCode2"] != DBNull.Value ? reader["ProductCode2"].ToString() : string.Empty,
                            BarCode = reader["BarCode"] != DBNull.Value ? reader["BarCode"].ToString() : string.Empty,
                            Name = reader["Περιγραφή"].ToString(),
                            FPA = int.Parse(reader["ΦΠΑ"] != DBNull.Value ? reader["ΦΠΑ"].ToString() : "0"),
                            Price = reader["ΤιμήΧονδρικής"] != DBNull.Value? double.Parse(reader["ΤιμήΧονδρικής"].ToString()) : 0,
                            LastPriceSold = reader["LastPrice"] != DBNull.Value ? double.Parse(reader["LastPrice"].ToString()) : 0,
                            Color = reader["Χρώματα"] != DBNull.Value ? reader["Χρώματα"].ToString() : string.Empty,
                            Size = reader["Μεγέθη"] != DBNull.Value ? reader["Μεγέθη"].ToString() : string.Empty,
                            Width = reader["Πλάτος"] != DBNull.Value ? float.Parse(reader["Πλάτος"].ToString()) : 0,
                            Length = reader["Μήκος"] != DBNull.Value ? float.Parse(reader["Μήκος"].ToString()) : 0,
                            Height = reader["Υψος"] != DBNull.Value ? float.Parse(reader["Υψος"].ToString()) : 0,
                            Type = int.Parse(reader["ΤύποςΔιάστασης"] != DBNull.Value ? reader["ΤύποςΔιάστασης"].ToString() : "0"),
                            UnitOfMeasure = string.IsNullOrEmpty(reader["ΜονάδαΜέτρησης"].ToString()) ? "Ποσότητα" : reader["ΜονάδαΜέτρησης"].ToString(),
                        });
                    }
                    return ProductsList;

                }
            });

        }
        public async Task<List<Product>> GetProductsFromSalesDoc(string doc)
        {
            return await Task.Run(async () =>
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(await GetParamAsync("getProductsFromSalesDoc"), doc);
                string queryString = sb.ToString();
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
                            ProductCode = reader["Κωδικός"] != DBNull.Value ? reader["Κωδικός"].ToString() : string.Empty,
                            BarCode = reader["BarCodeΕίδους"] != DBNull.Value ? reader["BarCodeΕίδους"].ToString() : string.Empty,
                            Name = reader["Περιγραφή"].ToString(),
                            Quantity = int.Parse(reader["Ποσότητα"].ToString()),
                        });
                    }
                    return ProductsList;

                }
            });

        }

        public Task<bool> DeleteItemFromDBAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
    //public async Task<List<Product>> GetItemsWithNameAsync(string name)
    //{
    //    return await Task.Run(() =>
    //    {
    //        string queryString = @"SELECT  BarCode
    //                              , BarCodeΕίδους.Περιγραφή as BarCodeDesc
    //                              ,Είδος.Oid
	   //                           ,Είδος.Περιγραφή
	   //                           ,Είδος.ΤιμήΧονδρικής
	   //                           ,Είδος.Κωδικός
	   //                           ,Είδος.ΦΠΑ
    //                              ,Είδος.Εκπτωση
	   //                           ,Χρώματα.Χρώματα
	   //                           ,Μεγέθη.Μεγέθη
    //                              ,BarCodeΕίδους.Πλάτος
    //                              ,BarCodeΕίδους.Μήκος
    //                              ,BarCodeΕίδους.Υψος
	   //                           ,ΜονάδεςΜέτρησης.ΜονάδαΜέτρησης
    //                              ,ΜονάδεςΜέτρησης.ΤύποςΔιάστασης
    //                          FROM BarCodeΕίδους
    //                          right join Είδος on BarCodeΕίδους.Είδος = Είδος.Oid
    //                          left join Χρώματα on Χρώματα.Oid = BarCodeΕίδους.Χρώμα
    //                          left join ΜονάδεςΜέτρησης on ΜονάδεςΜέτρησης.Oid = BarCodeΕίδους.ΜονάδαΜέτρησης
    //                          left join Μεγέθη on Μεγέθη.Oid = BarCodeΕίδους.Μέγεθος
                                 
    //                          where Είδος.Κωδικός LIKE '%" + name + "%' OR BarCode LIKE '%" + name + "%' OR  Είδος.Περιγραφή LIKE '%" + name + "%'";

    //        using (SqlConnection connection = new SqlConnection(ConnectionString))
    //        {
    //            /*ProductsList.Clear()*/
    //            ProductsList = new List<Product>();
    //            connection.Open();
    //            SqlCommand command = new SqlCommand(queryString, connection);
    //            SqlDataReader reader = command.ExecuteReader();


    //            while (reader.Read())
    //            {
    //                ProductsList.Add(new Product()
    //                {
    //                    Oid = Guid.Parse(reader["Oid"].ToString()),
    //                    ProductCode = reader["Κωδικός"] != null ? reader["Κωδικός"].ToString() : string.Empty,
    //                    Name = reader["Περιγραφή"].ToString(),
    //                    FPA = int.Parse(reader["ΦΠΑ"] != null ? reader["ΦΠΑ"].ToString() : "0"),
    //                    Price = double.Parse(reader["ΤιμήΧονδρικής"].ToString()),
    //                    Discount = int.Parse(reader["Εκπτωση"] != DBNull.Value ? reader["Εκπτωση"].ToString() : "0"),
    //                    BarCode = reader["BarCode"].ToString(),
    //                    BarCodeDesc = reader["BarCodeDesc"].ToString(),
    //                    Color = reader["Χρώματα"] != null ? reader["Χρώματα"].ToString() : string.Empty,
    //                    Size = reader["Μεγέθη"] != null ? reader["Μεγέθη"].ToString() : string.Empty,
    //                    Width = reader["Πλάτος"] != DBNull.Value ? float.Parse(reader["Πλάτος"].ToString()) : 0,
    //                    Length = reader["Μήκος"] != DBNull.Value ? float.Parse(reader["Μήκος"].ToString()) : 0,
    //                    Height = reader["Υψος"] != DBNull.Value ? float.Parse(reader["Υψος"].ToString()) : 0,
    //                    Type = int.Parse(reader["ΤύποςΔιάστασης"] != DBNull.Value ? reader["ΤύποςΔιάστασης"].ToString() : "0"),
    //                    UnitOfMeasure = string.IsNullOrEmpty(reader["ΜονάδαΜέτρησης"].ToString()) ? "Ποσότητα" : reader["ΜονάδαΜέτρησης"].ToString(),
    //                });
    //            }
    //            return ProductsList;

    //        }
    //    });

    //}
}
