using Smart_Orders_Project.Models.SparePartModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    public class RepositoryTreeGrouping :RepositoryService,IDataGet<Grouping>
    {
        List<Grouping> GroupingList;

        public RepositoryTreeGrouping()
        {
            GroupingList = new List<Grouping>();
        }
        public async Task<List<Grouping>> GetItemsWithNameAsync()
        {
            return await Task.Run(async () =>
            {
                GroupingList.Clear();
                
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(await GetParamAsync("getTreeGrouping"));
                string queryString = sb.ToString();

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
            return await Task.FromResult(GroupingList.Where(x => x.ParentOid == id).OrderBy(x => x.Name).ToList());
        }
        public async Task<Grouping> GetItemAsync(string id)
        {
            return await Task.FromResult(GroupingList.FirstOrDefault(s => s.Oid.ToString() == id));
        }

        public async Task<List<Grouping>> GetItemsAsync(bool forceRefresh = false)
        {
            if (!GroupingList.Any())
                await GetItemsWithNameAsync();

            return await Task.FromResult(GroupingList);
        }
    }
}
