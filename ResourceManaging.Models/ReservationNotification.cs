using System.ComponentModel.DataAnnotations;
namespace ResourceManaging.Models
{
    public class ReservationNotification
    {
        public int NotificationId { get; set; }

        [Required]
        public int ReservationId { get; set; }

        [Required]
        public char NotificationType { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime SentAt { get; set; }

        public bool IsRead { get; set; }

        [StringLength(255, MinimumLength = 3, ErrorMessage = "Message must be between 3 and 255 characters")]
        public string Message { get; set; }
        


    }
}