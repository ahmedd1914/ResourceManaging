using Microsoft.Data.SqlClient;
using ResourceManaging.Repository.Base;
using ResourceManaging.Models;
using ResourceManaging.Repository.Helpers;
using ResourceManaging.Repository.Interfaces.Reservation;

namespace ResourceManaging.Repository.Implementation.Reservations
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {   
        private const string IdDbFieldName = "ReservationId";
        private const string ReservorIdDbFieldName = "ReservorId";
        private const string ReservationResourcesTable = "ReservationResources";

        public ReservationRepository(string connectionString) 
            : base(connectionString, "Reservations", "ReservationId")
        {
        }

        protected override string[] GetColumns() => new[]{
            ReservorIdDbFieldName,
            "StartTime",
            "EndTime",
            "Purpose",
            "Participants",
            "IsCancelled",
            "CreatedAt"
        };

        protected override Reservation MapToEntity(SqlDataReader reader)
        {
            return new Reservation
            {
                ReservationId = GetValue<int>(reader, "ReservationId"),
                ReservorId = GetValue<int>(reader, "ReservorId"),
                StartTime = GetValue<DateTime>(reader, "StartTime"),
                EndTime = GetValue<DateTime>(reader, "EndTime"),
                Purpose = GetValue<string>(reader, "Purpose"),
                Participants = GetValue<int>(reader, "Participants"),
                IsCancelled = GetValue<bool>(reader, "IsCancelled"),
                CreatedAt = GetValue<DateTime>(reader, "CreatedAt")
            };
        }

        public async Task<int> CreateAsync(Models.Reservation entity, List<int> resourceIds)
        {
            using var connection = await CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Insert the reservation
                var command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = SqlQueryHelper.Insert.Build(_tableName, string.Join(",", GetColumns()));
                
                command.Parameters.AddWithValue("@ReservorId", entity.ReservorId);
                command.Parameters.AddWithValue("@StartTime", entity.StartTime);
                command.Parameters.AddWithValue("@EndTime", entity.EndTime);
                command.Parameters.AddWithValue("@Purpose", entity.Purpose);
                command.Parameters.AddWithValue("@Participants", entity.Participants);
                command.Parameters.AddWithValue("@IsCancelled", entity.IsCancelled);
                command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);

                var reservationIdObj = await command.ExecuteScalarAsync();
                if (reservationIdObj == null)
                {
                    transaction.Rollback();
                    return 0;
                }
                var reservationId = Convert.ToInt32(reservationIdObj);

                // Insert the reservation resources
                if (resourceIds != null && resourceIds.Any())
                {
                    foreach (var resourceId in resourceIds)
                    {
                        command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandText = $"INSERT INTO {ReservationResourcesTable} (ReservationId, ResourceId) VALUES (@ReservationId, @ResourceId)";
                        
                        command.Parameters.AddWithValue("@ReservationId", reservationId);
                        command.Parameters.AddWithValue("@ResourceId", resourceId);
                        
                        await command.ExecuteNonQueryAsync();
                    }
                }

                transaction.Commit();
                return reservationId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<Reservation>> RetrieveByFilterAsync(ReservationFilter filter)
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
            
            var reservations = new List<Reservation>();
            while (await reader.ReadAsync())
            {
                var reservation = MapToEntity(reader);
                reservations.Add(reservation);
            }
            return reservations;
        }

        public async Task<bool> UpdateAsync(ReservationUpdate update)
        {
            using var connection = await CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // At the start of the try block, declare result
                var result = false;

                // Update the reservation
                var command = connection.CreateCommand();
                command.Transaction = transaction;
                
                // Exclude "ResourceIds" from the update columns
                var updateKeys = update.GetUpdateKeysExcluding(_idColumnName).Where(k => k != "ResourceIds").ToList();

                // Only proceed with update if there are columns to update
                if (updateKeys.Any())
                {
                    command.CommandText = SqlQueryHelper.Update.Build(_tableName, 
                        string.Join(",", updateKeys), 
                        _idColumnName);
                    
                    foreach (var key in updateKeys)
                    {
                        command.Parameters.AddWithValue($"@{key}", update.GetUpdate<object>(key));
                    }
                    command.Parameters.AddWithValue($"@{_idColumnName}", update.GetUpdate<object>(_idColumnName));

                    result = await command.ExecuteNonQueryAsync() > 0;
                }

                // Update the reservation resources if ResourceIds is included in the update
                if (update.GetUpdateKeysExcluding(_idColumnName).Contains("ResourceIds"))
                {
                    // Delete existing resources
                    command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = $"DELETE FROM {ReservationResourcesTable} WHERE ReservationId = @ReservationId";
                    command.Parameters.AddWithValue("@ReservationId", update.GetUpdate<object>(_idColumnName));
                    await command.ExecuteNonQueryAsync();

                    // Insert new resources
                    var resourceIds = update.GetUpdate<List<int>>("ResourceIds");
                    foreach (var resourceId in resourceIds)
                    {
                        command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandText = $"INSERT INTO {ReservationResourcesTable} (ReservationId, ResourceId) VALUES (@ReservationId, @ResourceId)";
                        command.Parameters.AddWithValue("@ReservationId", update.GetUpdate<object>(_idColumnName));
                        command.Parameters.AddWithValue("@ResourceId", resourceId);
                        await command.ExecuteNonQueryAsync();
                    }
                }

                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using var connection = await CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Delete associated resources first
                var command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = $"DELETE FROM {ReservationResourcesTable} WHERE ReservationId = @ReservationId";
                command.Parameters.AddWithValue("@ReservationId", id);
                await command.ExecuteNonQueryAsync();

                // Delete the reservation
                command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = SqlQueryHelper.Delete.Build(_tableName, _idColumnName);
                command.Parameters.AddWithValue($"@{_idColumnName}", id);

                var result = await command.ExecuteNonQueryAsync() > 0;
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<int>> GetResourceIdsForReservationAsync(int reservationId)
        {
            var resourceIds = new List<int>();
            using var connection = await CreateConnectionAsync();
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT ResourceId FROM {ReservationResourcesTable} WHERE ReservationId = @ReservationId";
            command.Parameters.AddWithValue("@ReservationId", reservationId);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                resourceIds.Add(GetValue<int>(reader, "ResourceId"));
            }
            return resourceIds;
        }
    }
}