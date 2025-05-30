using ResourceManaging.Repository.Helpers;

namespace ResourceManaging.Repository.Interfaces.Reservation
{
    public class ReservationResourceUpdate : Update
    {
        public void UpdateResourceId(int resourceId)
        {
            AddUpdate("ResourceId", resourceId);
        }
    }
} 