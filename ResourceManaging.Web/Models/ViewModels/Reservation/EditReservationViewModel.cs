using System.ComponentModel.DataAnnotations;
using ResourceManaging.Web.Models.ViewModels.Resource;

namespace ResourceManaging.Web.Models
{
    public class EditReservationViewModel
    {
        public int ReservationId { get; set; }

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

        [Required]
        [MinLength(1, ErrorMessage = "At least one resource must be selected")]
        public List<int> SelectedResourceIds { get; set; } = new List<int>();

        public List<ResourceDetailsViewModel> AvailableResources { get; set; } = new List<ResourceDetailsViewModel>();
    }
} 