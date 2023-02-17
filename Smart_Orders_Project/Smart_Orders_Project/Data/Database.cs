using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SmartMobileWMS.Data
{
    public class Database
    {
        readonly SqlConnection _connection;
        public Database(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }
        public SqlConnection Connection { get => _connection; }
    }
}
