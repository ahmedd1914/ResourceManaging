using System.Data;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using ResourceManaging.Repository.Helpers;

namespace ResourceManaging.Repository.Base
{
    public abstract class BaseRepository<TObj> 
    {
        protected readonly string _connectionString;
        protected readonly string _tableName;
        protected readonly string _idColumnName;

        protected BaseRepository(string connectionString, string tableName, string idColumnName)
        {
            _connectionString = connectionString;
            _tableName = tableName;
            _idColumnName = idColumnName;
        }

        protected abstract string[] GetColumns();

        protected virtual string GetSelectColumns() =>
                string.Join(", ", typeof(TObj).GetProperties()
                .Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any())
                .Select(p => p.Name));

        protected abstract TObj MapToEntity(SqlDataReader reader);

        protected virtual void AddParameters(SqlCommand command, TObj entity, bool includeId = false)
        {
            var properties = typeof(TObj).GetProperties()
                .Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any());

            foreach (var prop in properties)
            {
                if (!includeId && prop.Name == _idColumnName)
                    continue;

                var value = prop.GetValue(entity);
                command.Parameters.AddWithValue("@" + prop.Name, value ?? DBNull.Value);
            }
        }

        protected async Task<SqlConnection> CreateConnectionAsync()
        {
            return await ConnectionFactory.CreateConnection();
        }

        public virtual async Task<TObj> RetrieveByIdAsync(int id)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = connection.CreateCommand();
                command.CommandText = SqlQueryHelper.Select.Build(_tableName, GetSelectColumns(), _idColumnName);
                command.Parameters.AddWithValue("@" + _idColumnName, id);

                using var reader = await command.ExecuteReaderAsync();
                return await reader.ReadAsync() ? MapToEntity(reader) : default;
            }
        }

        public virtual async Task<IEnumerable<TObj>> RetrieveAllAsync(string orderByColumn = null, bool ascending = true)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = connection.CreateCommand();
                command.CommandText = SqlQueryHelper.Select.BuildAll(_tableName, GetSelectColumns(), orderByColumn, ascending);

                using var reader = await command.ExecuteReaderAsync();
                var entities = new List<TObj>();
                while (await reader.ReadAsync())
                {
                    entities.Add(MapToEntity(reader));
                }
                return entities;
            }
        }

        public virtual async Task<int> CreateAsync(TObj entity)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = connection.CreateCommand();
                var columns = GetColumns().Where(c => c != _idColumnName);
                command.CommandText = SqlQueryHelper.Insert.Build(_tableName, string.Join(",", columns));
                AddParameters(command, entity, false);

                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
        }

        public virtual async Task<bool> UpdateAsync(TObj entity)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = connection.CreateCommand();
                command.CommandText = SqlQueryHelper.Update.Build(_tableName, string.Join(",", GetColumns()), _idColumnName);
                AddParameters(command, entity, true);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = connection.CreateCommand();
                command.CommandText = SqlQueryHelper.Delete.Build(_tableName, _idColumnName);
                command.Parameters.AddWithValue("@" + _idColumnName, id);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        protected TObj GetValue<TObj>(SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(ordinal))
            {
                if (typeof(TObj) == typeof(DateTime))
                    throw new InvalidOperationException($"Null DateTime value for column {columnName}");
                return default;
            }
            return (TObj)reader.GetValue(ordinal);
        }
    }

}