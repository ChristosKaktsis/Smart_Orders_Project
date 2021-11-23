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
   public class RepositoryCustomers : IDataStore<Customer>
    {
        public List<Customer> CustomerList { get; set; }
        private string ConnectionString
        {
            get => Preferences.Get(nameof(ConnectionString), @"User Id=sa;password=1;Pooling=false;Data Source=192.168.3.44\SQLEXPRESS;Initial Catalog=maindemo");
        }
        public RepositoryCustomers()
        {
            
        }

        private  void GetItemsFromDB()
        {
            string queryString = "select Oid , Κωδικός ,Επωνυμία ,ΑΦΜ ,Email from Πελάτης where GCRecord is null";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CustomerList.Add(new Customer
                    {
                        Oid = Guid.Parse(reader["Oid"].ToString()),
                        CodeNumber = reader["Κωδικός"] != null ? reader["Κωδικός"].ToString() : string.Empty,
                        Name = reader["Επωνυμία"].ToString(),
                        AFM = reader["ΑΦΜ"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
               
            }
        }

        public Task<bool> AddItemAsync(Customer item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> GetItemAsync(string id)
        {
            return await Task.Run(() =>
            {
                string queryString = "select Oid , Κωδικός ,Επωνυμία ,ΔιακριτικόςΤίτλος ,ΑΦΜ ,Email from Πελάτης where Oid ='" + id + "' and  GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    Customer customer = new Customer()
                    {
                        Oid = Guid.Parse(reader["Oid"].ToString()),
                        CodeNumber = reader["Κωδικός"] != null ? reader["Κωδικός"].ToString() : string.Empty,
                        Name = reader["Επωνυμία"].ToString(),
                        AFM = reader["ΑΦΜ"].ToString(),
                        Email = reader["Email"].ToString(),
                        AltName = reader["ΔιακριτικόςΤίτλος"].ToString()
                    };
                    return customer;
                }
            });
        }

        public async Task<List<Customer>> GetItemsAsync(bool forceRefresh = false)
        { 
            return await Task.Run(()=>
            {
                CustomerList = new List<Customer>();
                string queryString = "select Oid , Κωδικός ,Επωνυμία ,ΑΦΜ, ΔιακριτικόςΤίτλος ,Email from Πελάτης where GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        CustomerList.Add(new Customer
                        {
                            Oid = Guid.Parse(reader["Oid"].ToString()),
                            CodeNumber = reader["Κωδικός"] != null ? reader["Κωδικός"].ToString() : string.Empty,
                            Name = reader["Επωνυμία"].ToString(),
                            AFM = reader["ΑΦΜ"].ToString(),
                            Email = reader["Email"].ToString(),
                            AltName = reader["ΔιακριτικόςΤίτλος"].ToString()
                        });
                    }
                    return CustomerList;
                }
            });
        }

        public Task<bool> UpdateItemAsync(Customer item)
        {
            throw new NotImplementedException();
        }
        

        public Task<bool> UploadItemAsync(Customer item)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Customer>> GetItemsWithNameAsync(string name)
        {
            return await Task.Run(() =>
            {
                CustomerList = new List<Customer>();
                //CustomerList.Clear();
                string queryString = "select Oid , Κωδικός ,Επωνυμία ,ΑΦΜ, ΔιακριτικόςΤίτλος ,Email from Πελάτης where Κωδικός Like '%"+name+ "%' or Επωνυμία like '%" + name + "%' or ΑΦΜ like '%" + name + "%' and GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        CustomerList.Add(new Customer
                        {
                            Oid = Guid.Parse(reader["Oid"].ToString()),
                            CodeNumber = reader["Κωδικός"] != null ? reader["Κωδικός"].ToString() : string.Empty,
                            Name = reader["Επωνυμία"].ToString(),
                            AFM = reader["ΑΦΜ"].ToString(),
                            Email = reader["Email"].ToString(),
                            AltName = reader["ΔιακριτικόςΤίτλος"].ToString()
                        });
                    }
                    return CustomerList;
                }
            });
        }
    }
}
