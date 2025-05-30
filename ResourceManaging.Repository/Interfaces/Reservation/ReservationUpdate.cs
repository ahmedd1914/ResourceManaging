using System.ComponentModel.DataAnnotations;
using ResourceManaging.Repository.Helpers;

namespace ResourceManaging.Repository.Interfaces.Reservation
{
    public class ReservationUpdate : Update
    {
        private int _reservationId;
        public int ReservationId 
        { 
            get => _reservationId;
            set
            {
                _reservationId = value;
                AddUpdate("ReservationId", value);
            }
        }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Purpose must be between 3 and 255 characters")]
        public string Purpose { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Participants must be at least 1")]
        public int Participants { get; set; }

        [Required]
        public bool IsCancelled { get; set; }

        public List<int> ResourceIds { get; set; }

        public void UpdateStartTime(DateTime startTime)
        {
            AddUpdate("StartTime", startTime);
        }

        public void UpdateEndTime(DateTime endTime)
        {
            AddUpdate("EndTime", endTime);
        }

        public void UpdatePurpose(string purpose)
        {
            AddUpdate("Purpose", purpose);
        }

        public void UpdateParticipants(int participants)
        {
            AddUpdate("Participants", participants);
        }

        public void UpdateCancelledStatus(bool isCancelled)
        {
            AddUpdate("IsCancelled", isCancelled);
        }

        public void UpdateResourceIds(List<int> resourceIds)
        {
            AddUpdate("ResourceIds", resourceIds);
        }
        public void UpdateResources(List<int> resourceIds)
        {
            AddUpdate("ResourceIds", resourceIds);
        }
    }
} 