using Microsoft.Data.SqlClient;
using ResourceManaging.Repository.Base;
using ResourceManaging.Models;
using ResourceManaging.Repository.Helpers;
using ResourceManaging.Repository.Interfaces.Employee;

namespace ResourceManaging.Repository.Implementation.Employees
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        private const string IdDbFieldName = "EmployeeId";
        public EmployeeRepository(string connectionString) 
            : base(connectionString, "Employees", "EmployeeId")
        {
        }


        protected override string[] GetColumns() => new[]{
            IdDbFieldName,
            "Username",
            "Email",
            "PasswordHash",
            "FullName",
            "DateOfBirth"
        };
       

        protected override Employee MapToEntity(SqlDataReader reader)
        {
            return new Employee
            {
                EmployeeId = GetValue<int>(reader, IdDbFieldName),
                Username = GetValue<string>(reader, "Username"),
                Email = GetValue<string>(reader, "Email"),
                PasswordHash = GetValue<string>(reader, "PasswordHash"),
                FullName = GetValue<string>(reader, "FullName"),
                DateOfBirth = GetValue<DateTime>(reader, "DateOfBirth")
            };
        }

        public async Task<IEnumerable<Employee>> RetrieveByFilterAsync(EmployeeFilter filter)
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
            
            var employees = new List<Employee>();
            while (await reader.ReadAsync())
            {
                employees.Add(MapToEntity(reader));
            }
            return employees;
        }

        public async Task<bool> UpdateAsync(EmployeeUpdate update)
        {
            using var connection = await CreateConnectionAsync();
            var command = connection.CreateCommand();
            
            var updateKeys = update.GetUpdateKeysExcluding(_idColumnName).ToList();
            command.CommandText = SqlQueryHelper.Update.Build(_tableName, 
                string.Join(",", updateKeys), 
                _idColumnName);
            
            foreach (var key in updateKeys)
            {
                var value = update.GetUpdate<object>(key);
                command.Parameters.AddWithValue($"@{key}", value ?? DBNull.Value);
            }

            // Add the ID parameter for the WHERE clause
            var idValue = update.GetUpdate<object>(_idColumnName);
            command.Parameters.AddWithValue($"@{_idColumnName}", idValue ?? DBNull.Value);

            return await command.ExecuteNonQueryAsync() > 0;
        }
    }
} 