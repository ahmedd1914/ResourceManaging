using ResourceManaging.Repository.Helpers;

namespace ResourceManaging.Repository.Interfaces.Reservation
{
    public class ReservationFilter : Filter
    {
        public void AddReservorFilter(int reservorId)
        {
            AddParameter("ReservorId", reservorId);
        }

        public void AddDateRangeFilter(DateTime startDate, DateTime endDate)
        {
            AddParameter("StartTime", startDate);
            AddParameter("EndTime", endDate);
        }

        public void AddCancelledFilter(bool isCancelled)
        {
            AddParameter("IsCancelled", isCancelled);
        }
        
    }
} 