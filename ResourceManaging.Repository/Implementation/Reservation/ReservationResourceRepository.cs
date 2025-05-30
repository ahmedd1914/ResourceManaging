using ResourceManaging.Repository.Base;
using ResourceManaging.Models;
using ResourceManaging.Repository.Interfaces.Reservation;
using Microsoft.Data.SqlClient;
using ResourceManaging.Repository.Helpers;

namespace ResourceManaging.Repository.Implementation.Reservations
{
    public class ReservationResourceRepository : BaseRepository<ReservationResource>, IReservationResourceRepository    
    {
        private const string ReservationIdDbFieldName = "ReservationId";
        private const string ResourceIdDbFieldName = "ResourceId";
        public ReservationResourceRepository(string connectionString) : base(connectionString, "ReservationResources", ReservationIdDbFieldName)
        {
        }

        protected override string[] GetColumns() => new[]{
            ReservationIdDbFieldName,
            ResourceIdDbFieldName
        };

        protected override ReservationResource MapToEntity(SqlDataReader reader)
        {
            return new ReservationResource
            {
                ReservationId = GetValue<int>(reader, ReservationIdDbFieldName),
                ResourceId = GetValue<int>(reader, ResourceIdDbFieldName)
            };
        }

        public override async Task<int> CreateAsync(ReservationResource entity)
        {
            using var connection = await CreateConnectionAsync();
            var command = connection.CreateCommand();
            
            command.CommandText = SqlQueryHelper.Insert.Build(_tableName, string.Join(",", GetColumns()));
            
            command.Parameters.AddWithValue($"@{ReservationIdDbFieldName}", entity.ReservationId);
            command.Parameters.AddWithValue($"@{ResourceIdDbFieldName}", entity.ResourceId);

            return await command.ExecuteNonQueryAsync() > 0 ? entity.ReservationId : 0;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using var connection = await CreateConnectionAsync();
            var command = connection.CreateCommand();
            
            command.CommandText = $"DELETE FROM {_tableName} WHERE {ReservationIdDbFieldName} = @{ReservationIdDbFieldName}";
            command.Parameters.AddWithValue($"@{ReservationIdDbFieldName}", id);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<IEnumerable<ReservationResource>> RetrieveByFilterAsync(ReservationResourceFilter filter)
        {
            using var connection = await CreateConnectionAsync();
            var command = connection.CreateCommand();
            
            var query = new System.Text.StringBuilder();
            query.Append(SqlQueryHelper.Select.BuildAll(_tableName, string.Join(",", GetColumns())));
            
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
            
            var resources = new List<ReservationResource>();
            while (await reader.ReadAsync())
            {
                resources.Add(MapToEntity(reader));
            }
            return resources;
        }

        public async Task<bool> UpdateAsync(ReservationResourceUpdate update)
        {
            using var connection = await CreateConnectionAsync();
            var command = connection.CreateCommand();
            
            command.CommandText = SqlQueryHelper.Update.Build(_tableName, 
                string.Join(",", update.GetUpdates<string>()), 
                ReservationIdDbFieldName);
            
            foreach (var key in update.GetUpdates<string>())
            {
                command.Parameters.AddWithValue($"@{key}", update.GetUpdate<object>(key));
            }

            command.Parameters.AddWithValue($"@{ReservationIdDbFieldName}", update.GetUpdate<object>(ReservationIdDbFieldName));

            return await command.ExecuteNonQueryAsync() > 0;
        }
    }
}