using ResourceManaging.Web.Models;

public class HomeSummaryViewModel
{
    public int TotalResources { get; set; }
    public int TotalReservations { get; set; }
    public int TotalActiveResources { get; set; }

    public List<ReservationDetailsViewModel> RecentReservations { get; set; }
    public List<ResourceInfo> RecentResources { get; set; }
}