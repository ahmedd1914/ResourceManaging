using ResourceManaging.Repository.Base;
using ResourceManaging.Repository.Implementation.Reservations;

namespace ResourceManaging.Repository.Interfaces.Reservation
{
    public interface IReservationRepository : IBaseRepository<Models.Reservation, ReservationFilter, ReservationUpdate>
    {
        Task<List<int>> GetResourceIdsForReservationAsync(int reservationId);
        Task<int> CreateAsync(Models.Reservation reservation, List<int> resourceIds);
    }
}