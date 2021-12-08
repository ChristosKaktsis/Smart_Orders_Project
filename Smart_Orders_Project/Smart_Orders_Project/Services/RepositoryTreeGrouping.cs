using Smart_Orders_Project.Models.SparePartModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
                string queryString = $@"SELECT a.Oid
                                      ,a.Parent
                                      ,a.Name as NameChild
	                                  ,ΔενδρικήΟμαδοποιησηΕιδών.ID   
  FROM HCategory a   inner join ΔενδρικήΟμαδοποιησηΕιδών on ΔενδρικήΟμαδοποιησηΕιδών.Oid = a.Oid
";
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
                            Name = reader["NameChild"] == DBNull.Value ? "" : reader["NameChild"].ToString(),
                            ID = reader["ID"] == DBNull.Value ? "" : reader["ID"].ToString(),
                            ParentOid = reader["Parent"] == DBNull.Value ? string.Empty : reader["Parent"].ToString(),
                        });
                    }

                    return GroupingList;
                }
            });
        }

        public async Task<List<Grouping>> GetItemChildrenAsync(string id)
        {
            return await Task.FromResult(GroupingList.Where(x => x.ParentOid == id).ToList());
        }
    }
}
