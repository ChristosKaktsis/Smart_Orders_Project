using SmartMobileWMS.Models.SparePartModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.Services
{
    public class RepositoryModel : RepositoryService , IDataStore<Model>
    {
        List<Model> ModelList;
        public RepositoryModel()
        {
            ModelList = new List<Model>();
        }

        public Task<bool> AddItemAsync(Model item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemFromDBAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Model> GetItemAsync(string id)
        {
            return await Task.FromResult(ModelList.FirstOrDefault(s => s.Oid.ToString() == id));
        }

        public async Task<List<Model>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(ModelList.OrderBy(x => x.Description).ToList());
        }

        public async Task<List<Model>> GetItemsWithNameAsync(string name)
        {
            ModelList.Clear();
            return await Task.Run(async() =>
            {
                string oldqueryString = $@"SELECT Oid
                                        ,ΙεραρχικόΖοομ1
                                        ,Κωδικός
                                        ,Περιγραφή 
                                        FROM ΙεραρχικόΖοομ2
                                        where ΙεραρχικόΖοομ1 ='{name}' and GCRecord is null
                                    ";
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(await GetParamAsync("getModelsByBrand"), name);
                string queryString = sb.ToString();

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ModelList.Add(new Model()
                        {
                            Oid = Guid.Parse(reader["Oid"].ToString()),
                            ModelCode = reader["Κωδικός"] != DBNull.Value ? reader["Κωδικός"].ToString() : string.Empty,
                            Description = reader["Περιγραφή"] != DBNull.Value ? reader["Περιγραφή"].ToString() : string.Empty,
                            Brand = reader["ΙεραρχικόΖοομ1"] != DBNull.Value ? reader["ΙεραρχικόΖοομ1"].ToString() : string.Empty,
                        });
                    }
                    return ModelList.OrderBy(x => x.Description).ToList();
                }
            });
        }

        public Task<bool> UpdateItemAsync(Model item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UploadItemAsync(Model item)
        {
            throw new NotImplementedException();
        }
    }
}
