using ResourceManaging.Services.DTOs.Reservation;
using ResourceManaging.Repository.Implementation.Reservations;
using ResourceManaging.Repository.Interfaces.Reservation;

namespace ResourceManaging.Services.Interfaces.Reservation
{
    public interface IReservationNotificationService
    {
        /// <summary>
        /// Gets a notification by its ID
        /// </summary>
        /// <param name="notificationId">Notification ID</param>
        /// <returns>Notification data</returns>
        Task<ReservationNotificationResponse> GetNotificationByIdAsync(int notificationId);

        /// <summary>
        /// Gets notifications for a specific reservation
        /// </summary>
        /// <param name="reservationId">Reservation ID</param>
        /// <returns>List of notifications for the reservation</returns>
        Task<ReservationNotificationListResponse> GetNotificationsByReservationIdAsync(int reservationId);

        /// <summary>
        /// Gets notifications based on filter criteria
        /// </summary>
        /// <param name="filter">Filter criteria for notifications</param>
        /// <returns>List of notifications matching the filter</returns>
        Task<ReservationNotificationListResponse> GetNotificationsByFilterAsync(ResourceManaging.Repository.Interfaces.Reservation.ReservationNotificationFilter filter);

        /// <summary>
        /// Creates a new notification
        /// </summary>
        /// <param name="request">Notification creation request</param>
        /// <returns>Created notification data</returns>
        Task<ReservationNotificationResponse> CreateNotificationAsync(CreateReservationNotificationRequest request);

        /// <summary>
        /// Marks a notification as read
        /// </summary>
        /// <param name="notificationId">Notification ID</param>
        /// <returns>True if update was successful</returns>
        Task<ReservationNotificationResponse> MarkNotificationAsReadAsync(int notificationId);

        /// <summary>
        /// Updates notification details
        /// </summary>
        /// <param name="update">Update data containing notification ID and fields to update</param>
        /// <returns>True if update was successful</returns>
        Task<ReservationNotificationResponse> UpdateNotificationAsync(ReservationNotificationUpdate update);

        /// <summary>
        /// Deletes a notification
        /// </summary>
        /// <param name="notificationId">Notification ID</param>
        /// <returns>True if deletion was successful</returns>
        Task<ReservationNotificationResponse> DeleteNotificationAsync(int notificationId);
    }
}
