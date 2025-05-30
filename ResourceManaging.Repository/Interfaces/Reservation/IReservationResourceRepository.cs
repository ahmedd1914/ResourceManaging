using ResourceManaging.Repository.Base;
using ResourceManaging.Models;

namespace ResourceManaging.Repository.Interfaces.Reservation
{
    public interface IReservationResourceRepository : IBaseRepository<ReservationResource, ReservationResourceFilter, ReservationResourceUpdate>
    {
    }
}