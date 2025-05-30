using ResourceManaging.Repository.Base;
using ResourceManaging.Repository.Implementation.Resources;
using ResourceManaging.Repository.Interfaces.Resource;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using ResourceManaging.Repository.Helpers;
using System.Collections.Generic;

namespace ResourceManaging.Repository.Implementation.Resources
{
    public class ResourceTypeRepository : BaseRepository<Models.ResourceType>, IResourceTypeRepository
    {
        private const string IdDbFieldName = "ResourceTypeId";
        public ResourceTypeRepository(string connectionString) : base(connectionString, "ResourceTypes", "ResourceTypeId")
        {
        }

        protected override string[] GetColumns() => new[]{
            IdDbFieldName,
            "Name",
            "Description"
        };

        protected override Models.ResourceType MapToEntity(SqlDataReader reader){
            return new Models.ResourceType{
                ResourceTypeId = GetValue<int>(reader, "ResourceTypeId"),
                Name = GetValue<string>(reader, "Name"),
                Description = GetValue<string>(reader, "Description"),
            };
        }

        public async Task<bool> UpdateAsync(ResourceTypeUpdate update)
        {
            using var connection = await CreateConnectionAsync();
            var command = connection.CreateCommand();
            
            command.CommandText = SqlQueryHelper.Update.Build(_tableName, 
                string.Join(",", update.GetUpdateKeysExcluding(_idColumnName)), 
                _idColumnName);
            
            foreach (var key in update.GetUpdateKeysExcluding(_idColumnName))
            {
                command.Parameters.AddWithValue($"@{key}", update.GetUpdate<object>(key));
            }

            // Add the ID parameter for the WHERE clause
            command.Parameters.AddWithValue($"@{_idColumnName}", update.GetUpdate<object>(_idColumnName));

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<IEnumerable<Models.ResourceType>> RetrieveByFilterAsync(ResourceTypeFilter filter)
        {
            using var connection = await CreateConnectionAsync();
            var command = connection.CreateCommand();
            
            var query = new System.Text.StringBuilder();
            query.Append(SqlQueryHelper.Select.BuildAll(_tableName, GetSelectColumns()));
            
            if (filter.Count > 0)
            {
                query.AppendFormat(SqlQueryHelper.QueryTemplates.Where, 
                    SqlQueryHelper.QueryParts.BuildWhereConditions(filter.GetParameters()));
                
                foreach (var param in filter.GetParameters())
                {
                    command.Parameters.AddWithValue($"@{param}", filter.GetParameter<object>(param));
                }
            }

            command.CommandText = query.ToString();
            using var reader = await command.ExecuteReaderAsync();
            
            var types = new List<Models.ResourceType>();
            while (await reader.ReadAsync())
            {
                types.Add(MapToEntity(reader));
            }
            return types;
        }
    }
}