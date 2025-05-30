using ResourceManaging.Services.DTOs.Reservation;
using ResourceManaging.Repository.Interfaces.Reservation;

namespace ResourceManaging.Services.Interfaces.Reservation
{
    public interface IReservationService
    {
        /// <summary>
        /// Gets a reservation by its ID
        /// </summary>
        /// <param name="id">Reservation ID</param>
        /// <returns>Reservation response with reservation data</returns>
        Task<ReservationResponse> GetReservationByIdAsync(int id);

        /// <summary>
        /// Gets reservations based on filter criteria
        /// </summary>
        /// <param name="filter">Filter criteria for reservations</param>
        /// <returns>List of reservations matching the filter</returns>
        Task<ReservationListResponse> GetReservationsByFilterAsync(ReservationFilter filter);

        /// <summary>
        /// Creates a new reservation
        /// </summary>
        /// <param name="request">Reservation creation request</param>
        /// <returns>Created reservation data</returns>
        Task<ReservationResponse> CreateReservationAsync(CreateReservationRequest request);

        /// <summary>
        /// Updates an existing reservation
        /// </summary>
        /// <param name="request">Update request containing reservation ID and fields to update</param>
        /// <returns>True if update was successful</returns>
        Task<(bool Success, string Message)> UpdateReservationAsync(UpdateReservationRequest request);

        /// <summary>
        /// Deletes a reservation
        /// </summary>
        /// <param name="id">Reservation ID</param>
        /// <returns>True if deletion was successful</returns>
        Task<bool> DeleteReservationAsync(int id);

        /// <summary>
        /// Releases a reservation early
        /// </summary>
        /// <param name="reservationId">Reservation ID</param>
        /// <returns>True if release was successful</returns>
        Task<bool> ReleaseReservationEarlyAsync(int reservationId);
    }
}
