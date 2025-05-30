using ResourceManaging.Services.Interfaces.Reservation;
using ResourceManaging.Services.DTOs.Reservation;
using ResourceManaging.Repository.Interfaces.Reservation;
using ResourceManaging.Models;
using ServiceFilter = ResourceManaging.Services.DTOs.Reservation.ReservationNotificationFilter;

namespace ResourceManaging.Services.Implementations.Reservation
{
    public class ReservationNotificationService : IReservationNotificationService
    {
        private readonly IReservationNotificationRepository _notificationRepository;

        public ReservationNotificationService(IReservationNotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<ReservationNotificationResponse> GetNotificationByIdAsync(int id)
        {
            var notification = await _notificationRepository.RetrieveByIdAsync(id);
            if (notification == null)
            {
                return new ReservationNotificationResponse
                {
                    Success = false,
                    Message = "Notification not found"
                };
            }

            return new ReservationNotificationResponse
            {
                Success = true,
                Message = "Notification retrieved successfully",
                Notification = ReservationNotificationData.FromNotification(notification)
            };
        }

        public async Task<ReservationNotificationListResponse> GetNotificationsByFilterAsync(ResourceManaging.Repository.Interfaces.Reservation.ReservationNotificationFilter filter)
        {
            var notifications = await _notificationRepository.RetrieveByFilterAsync(filter);
            return new ReservationNotificationListResponse
            {
                Success = true,
                Message = "Notifications retrieved successfully",
                Notifications = notifications.Select(ReservationNotificationData.FromNotification).ToList(),
                TotalCount = notifications.Count()
            };
        }

        public async Task<ReservationNotificationListResponse> GetNotificationsByReservationIdAsync(int reservationId)
        {
            var filter = new ResourceManaging.Repository.Interfaces.Reservation.ReservationNotificationFilter();
            filter.AddReservationFilter(reservationId);
            return await GetNotificationsByFilterAsync(filter);
        }

        public async Task<ReservationNotificationResponse> CreateNotificationAsync(CreateReservationNotificationRequest request)
        {
            var notification = new Models.ReservationNotification
            {
                ReservationId = request.ReservationId,
                NotificationType = request.NotificationType,
                Message = request.Message,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            var id = await _notificationRepository.CreateAsync(notification);
            if (id <= 0)
            {
                return new ReservationNotificationResponse
                {
                    Success = false,
                    Message = "Failed to create notification"
                };
            }

            notification.NotificationId = id;
            return new ReservationNotificationResponse
            {
                Success = true,
                Message = "Notification created successfully",
                Notification = ReservationNotificationData.FromNotification(notification)
            };
        }

        public async Task<ReservationNotificationResponse> MarkNotificationAsReadAsync(int id)
        {
            var notification = await _notificationRepository.RetrieveByIdAsync(id);
            if (notification == null)
            {
                return new ReservationNotificationResponse
                {
                    Success = false,
                    Message = "Notification not found"
                };
            }

            var update = new ReservationNotificationUpdate();
            update.NotificationId = id;
            update.UpdateReadStatus(true);

            var success = await _notificationRepository.UpdateAsync(update);
            if (!success)
            {
                return new ReservationNotificationResponse
                {
                    Success = false,
                    Message = "Failed to mark notification as read"
                };
            }

            notification.IsRead = true;
            return new ReservationNotificationResponse
            {
                Success = true,
                Message = "Notification marked as read successfully",
                Notification = ReservationNotificationData.FromNotification(notification)
            };
        }

        public async Task<ReservationNotificationResponse> UpdateNotificationAsync(ReservationNotificationUpdate update)
        {
            var success = await _notificationRepository.UpdateAsync(update);
            if (!success)
            {
                return new ReservationNotificationResponse
                {
                    Success = false,
                    Message = "Failed to update notification"
                };
            }

            return new ReservationNotificationResponse
            {
                Success = true,
                Message = "Notification updated successfully"
            };
        }

        public async Task<ReservationNotificationResponse> DeleteNotificationAsync(int id)
        {
            var success = await _notificationRepository.DeleteAsync(id);
            if (!success)
            {
                return new ReservationNotificationResponse
                {
                    Success = false,
                    Message = "Failed to delete notification"
                };
            }

            return new ReservationNotificationResponse
            {
                Success = true,
                Message = "Notification deleted successfully"
            };
        }
    }
}
