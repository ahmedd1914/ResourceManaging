using ResourceManaging.Repository.Helpers;

namespace ResourceManaging.Repository.Interfaces.Reservation
{
    public class ReservationResourceFilter : Filter
    {
        public void AddReservationFilter(int reservationId)
        {
            AddParameter("ReservationId", reservationId);
        }

        public void AddResourceFilter(int resourceId)
        {
            AddParameter("ResourceId", resourceId);
        }
    }
} 