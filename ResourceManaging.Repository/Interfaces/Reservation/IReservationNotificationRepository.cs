using ResourceManaging.Repository.Base;
using ResourceManaging.Repository.Implementation.Reservations;

namespace ResourceManaging.Repository.Interfaces.Reservation
{
    public interface IReservationNotificationRepository : IBaseRepository<Models.ReservationNotification, ReservationNotificationFilter, ReservationNotificationUpdate>
    {
    }
}