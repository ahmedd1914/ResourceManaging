using ResourceManaging.Repository.Base;
using ResourceManaging.Models;
using ResourceManaging.Repository.Interfaces.Reservation;
using Microsoft.Data.SqlClient;
using ResourceManaging.Repository.Helpers;

namespace ResourceManaging.Repository.Implementation.Reservations
{
    public class ReservationNotificationRepository : BaseRepository<Models.ReservationNotification>, IReservationNotificationRepository
    {
        private const string IdDbFieldName = "NotificationId";
        private const string ReservationIdDbFieldName = "ReservationId";
        private const string NotificationTypeDbFieldName = "NotificationType";
        private const string SentAtDbFieldName = "SentAt";
        private const string IsReadDbFieldName = "IsRead";
        private const string MessageDbFieldName = "Message";

        public ReservationNotificationRepository(string connectionString) : base(connectionString, "ReservationNotifications", IdDbFieldName)
        {
        }

        protected override string[] GetColumns() => new[]{
            IdDbFieldName,
            ReservationIdDbFieldName,
            NotificationTypeDbFieldName,
            SentAtDbFieldName,
            IsReadDbFieldName,
            MessageDbFieldName
        };

        protected override ReservationNotification MapToEntity(SqlDataReader reader)
        {
            return new Models.ReservationNotification
            {
                NotificationId = GetValue<int>(reader, IdDbFieldName),
                ReservationId = GetValue<int>(reader, ReservationIdDbFieldName),
                NotificationType = GetValue<string>(reader, NotificationTypeDbFieldName)[0],
                SentAt = GetValue<DateTime>(reader, SentAtDbFieldName),
                IsRead = GetValue<bool>(reader, IsReadDbFieldName),
                Message = GetValue<string>(reader, MessageDbFieldName)
            };
        }

        public async Task<IEnumerable<ReservationNotification>> RetrieveByFilterAsync(ReservationNotificationFilter filter)
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
            
            var notifications = new List<ReservationNotification>();
            while (await reader.ReadAsync())
            {
                notifications.Add(MapToEntity(reader));
            }
            return notifications;
        }

        public async Task<bool> UpdateAsync(ReservationNotificationUpdate update)
        {
            using var connection = await CreateConnectionAsync();
            var command = connection.CreateCommand();

            // Use only the keys (column names) for the update statement
            var updateKeys = update.GetUpdateKeysExcluding(IdDbFieldName).ToList();
            command.CommandText = SqlQueryHelper.Update.Build(_tableName,
                string.Join(",", updateKeys),
                IdDbFieldName);

            foreach (var key in updateKeys)
            {
                command.Parameters.AddWithValue($"@{key}", update.GetUpdate<object>(key));
            }

            command.Parameters.AddWithValue($"@{IdDbFieldName}", update.GetUpdate<object>(IdDbFieldName));

            return await command.ExecuteNonQueryAsync() > 0;
        }
    }
}