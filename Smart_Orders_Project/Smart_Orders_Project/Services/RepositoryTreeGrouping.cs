using Smart_Orders_Project.Models.SparePartModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    class RepositoryTreeGrouping 
    {
        List<Grouping> GroupingList;

        public RepositoryTreeGrouping()
        {
            GroupingList = new List<Grouping>();
        }
        public string ConnectionString
        {
            get =>  @"User ID=sa;Password=1;Pooling=false;Data Source=192.168.3.99\SQLEXPRESS;Initial Catalog=SmartClearAntallaktiko";
           
        }
        public async Task<List<Grouping>> GetItemsWithNameAsync(string name)
        {
            return await Task.Run( () =>
            {
                GroupingList.Clear();
                string queryString = $@"select ΔενδρικήΟμαδοποιησηΕιδών.Oid, HCategory.Name, HCategory.Parent, ΔενδρικήΟμαδοποιησηΕιδών.ID
                                        from ΔενδρικήΟμαδοποιησηΕιδών
                                        inner join HCategory on ΔενδρικήΟμαδοποιησηΕιδών.Oid = HCategory.Oid
                                        where HCategory.GCRecord is null";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        GroupingList.Add(new Grouping
                        {
                            Oid = Guid.Parse(reader["Oid"].ToString()),
                            Name = reader["Name"] == DBNull.Value ? "" : reader["Name"].ToString(),
                            ID = reader["ID"] == DBNull.Value ? "" : reader["ID"].ToString(),
                        });
                    }
                    
                    return GroupingList;
                }
            });
        }
    }
}
