using ResourceManaging.Services.DTOs.Reservation;
using ResourceManaging.Repository.Interfaces.Reservation;

namespace ResourceManaging.Services.Interfaces.Reservation
{
    public interface IReservationResourceService
    {
        /// <summary>
        /// Gets resources for a specific reservation
        /// </summary>
        /// <param name="reservationId">Reservation ID</param>
        /// <returns>List of resources associated with the reservation</returns>
        Task<ReservationResourceListResponse> GetResourcesByReservationIdAsync(int reservationId);

        /// <summary>
        /// Gets reservations for a specific resource
        /// </summary>
        /// <param name="resourceId">Resource ID</param>
        /// <returns>List of reservations using the resource</returns>
        Task<ReservationResourceListResponse> GetReservationsByResourceIdAsync(int resourceId);

        /// <summary>
        /// Gets reservation resources based on filter criteria
        /// </summary>
        /// <param name="filter">Filter criteria for reservation resources</param>
        /// <returns>List of reservation resources matching the filter</returns>
        Task<ReservationResourceListResponse> GetReservationResourcesByFilterAsync(ReservationResourceFilter filter);

        /// <summary>
        /// Adds a resource to a reservation
        /// </summary>
        /// <param name="request">Request containing reservation and resource IDs</param>
        /// <returns>True if addition was successful</returns>
        Task<ReservationResourceResponse> AddResourceToReservationAsync(AddResourceToReservationRequest request);

        /// <summary>
        /// Removes a resource from a reservation
        /// </summary>
        /// <param name="request">Request containing reservation and resource IDs</param>
        /// <returns>True if removal was successful</returns>
        Task<bool> RemoveResourceFromReservationAsync(RemoveResourceFromReservationRequest request);

        /// <summary>
        /// Updates reservation resource details
        /// </summary>
        /// <param name="request">Update data containing reservation ID and fields to update</param>
        /// <returns>True if update was successful</returns>
        Task<bool> UpdateReservationResourceAsync(UpdateReservationResourceRequest request);
    }
}
