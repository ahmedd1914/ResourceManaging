using Microsoft.Data.SqlClient;
using ResourceManaging.Repository.Base;
using ResourceManaging.Models;
using ResourceManaging.Repository.Helpers;
using ResourceManaging.Repository.Interfaces.Resource;

namespace ResourceManaging.Repository.Implementation.Resources
{
    public class ResourceRepository : BaseRepository<Resource>, IResourceRepository
    {
        private const string IdDbFieldName = "ResourceId";
        private const string ResourceTypeIdDbFieldName = "ResourceTypeId";
        public ResourceRepository(string connectionString) 
            : base(connectionString, "Resources", "ResourceId")
        {
        }

        protected override string[] GetColumns() => new[]{
            IdDbFieldName,
            ResourceTypeIdDbFieldName,
            "Name",
            "Capacity",
            "IsActive"
        };

        protected override Resource MapToEntity(SqlDataReader reader)
        {
            return new Resource
            {
                ResourceId = GetValue<int>(reader, "ResourceId"),
                ResourceTypeId = GetValue<int>(reader, "ResourceTypeId"),
                Name = GetValue<string>(reader, "Name"),
                Capacity = GetValue<int>(reader, "Capacity"),
                IsActive = GetValue<bool>(reader, "IsActive")
            };
        }

        public async Task<IEnumerable<Resource>> RetrieveByFilterAsync(ResourceFilter filter)
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
            
            var resources = new List<Resource>();
            while (await reader.ReadAsync())
            {
                resources.Add(MapToEntity(reader));
            }
            return resources;
        }

        public async Task<bool> UpdateAsync(ResourceUpdate update)
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
    }
}