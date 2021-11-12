using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    class RepositoryRFSales : IDataStore<RFSales>
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
                
                RFSalesList.Clear();
                string queryString = "select Oid , Πελάτης ,UpdSmart ,Ολοκληρώθηκε ,ΗμνίαΔημιουργίας from RFΠωλήσεις where UpdSmart = 'false' and GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        
                        RFSalesList.Add(new RFSales
                        {
                            Oid = Guid.Parse(reader["Oid"].ToString()),
                            Customer =  await GetCustomer(reader["Πελάτης"].ToString()),
                            CreationDate = (DateTime)reader["ΗμνίαΔημιουργίας"],
                            Lines = await GetLines(reader["Oid"].ToString())
                        });
                    }
                    return RFSalesList;
                }
            });
        }

        private async Task<List<LineOfOrder>> GetLines(string id)
        {
            return await Task.Run(async () =>
            {
                var LineList = new List<LineOfOrder>();
                string queryString = "select Oid, RFΠωλήσεις , Είδος ,Ποσότητα ,BarCodeΕίδους ,ΠοσότηταΔιάστασης from RFΓραμμέςΠωλήσεων where RFΠωλήσεις ='"+id+"' and GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        LineList.Add(new LineOfOrder
                        {
                            Oid = Guid.Parse(reader["Oid"].ToString()),
                            Product = await GetProduct(reader["Είδος"].ToString()),
                            ProductBarCode = reader["BarCodeΕίδους"].ToString(),
                            Quantity = int.Parse(reader["Ποσότητα"].ToString()),
                            RFSalesOid = Guid.Parse(reader["RFΠωλήσεις"].ToString()),
                        });
                    }
                    return LineList;
                }
            });
        }

        private async Task<Product> GetProduct(string id)
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
	                              ,Χρώματα.Χρώματα
	                              ,Μεγέθη.Μεγέθη
	                              ,ΜονάδεςΜέτρησης.ΜονάδαΜέτρησης
                              FROM BarCodeΕίδους
                              join Είδος on BarCodeΕίδους.Είδος = Είδος.Oid
                              left join Χρώματα on Χρώματα.Oid = BarCodeΕίδους.Χρώμα
                              left join ΜονάδεςΜέτρησης on ΜονάδεςΜέτρησης.Oid = BarCodeΕίδους.ΜονάδαΜέτρησης
                              left join Μεγέθη on Μεγέθη.Oid = BarCodeΕίδους.Μέγεθος
                                 
                              where Είδος.Oid = '" + id + "'";

                using (SqlConnection connection = new SqlConnection(ConnectionString()))
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
                        BarCode = reader["BarCode"].ToString(),
                        BarCodeDesc = reader["BarCodeDesc"].ToString(),
                        Color = reader["Χρώματα"] != null ? reader["Χρώματα"].ToString() : string.Empty,
                        Size = reader["Μεγέθη"] != null ? reader["Μεγέθη"].ToString() : string.Empty,
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

                using (SqlConnection connection = new SqlConnection(ConnectionString()))
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
            //var oldItem = RFSalesList.Where((RFSales arg) => arg.Oid == item.Oid).FirstOrDefault();
            //RFSalesList.Remove(oldItem);
            //RFSalesList.Add(item);
            //return await Task.FromResult(true);
            return await Task.Run(() =>
            {
                int ok = 0;
                string queryString = @"UPDATE RFΠωλήσεις
                                    SET Πελάτης = 'CEA01ABC-F96E-4ADB-B6C4-0130D9F641DE'
                                    WHERE Πελάτης = '9095A026-D442-4724-AD75-3824B836C83D' ";
                using (SqlConnection connection = new SqlConnection(ConnectionString()))
                {
                    connection.Open();
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
                string queryString = @"INSERT INTO RFΠωλήσεις (Oid, ΑποθηκευτικόςΧώρος, Πελάτης, ΠαραστατικάΠωλήσεων, ΠαραστατικόΠελάτη, 
                    Διαχείριση, UpdSmart, Ολοκληρώθηκε, ΗμνίαΔημιουργίας, ΑυτόματηΔιαγραφήΠαραστατικών, OptimisticLockField, GCRecord)
                    VALUES((Convert(uniqueidentifier, N'"+item.Oid+ "')), null, (Convert(uniqueidentifier, N'" + item.Customer.Oid + "')), null, null, null, '0', '0', (Convert(date, '" + item.CreationDate.ToString("MM/dd/yyyy") + "')), '0', '1', null); ";
                using (SqlConnection connection = new SqlConnection(ConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    ok = command.ExecuteNonQuery();
                }
                return ok >= 1 ? true : false ;
            });
        }
        private string ConnectionString()
        {
            return @"User Id=sa;password=1;Pooling=false;Data Source=192.168.3.44\SQLEXPRESS;Initial Catalog=maindemo";
        }
    }
}
