using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResourceManaging.Services.Interfaces.Resource;
using ResourceManaging.Services.Interfaces.Reservation;
using ResourceManaging.Web.Models;
using ResourceManaging.Repository.Interfaces.Resource;
using ResourceManaging.Repository.Interfaces.Reservation;
using System.Linq;
using ResourceManaging.Web.Attributes;
namespace ResourceManaging.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IResourceService _resourceService;
        private readonly IReservationService _reservationService;

        public HomeController(
            ILogger<HomeController> logger,
            IResourceService resourceService,
            IReservationService reservationService)
        {
            _logger = logger;
            _resourceService = resourceService;
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index()
        {
            var resourcesResponse = await _resourceService.GetResourcesByFilterAsync(new ResourceFilter());
            var reservationsResponse = await _reservationService.GetReservationsByFilterAsync(new ReservationFilter());

            // Example: Count only active resources
            int totalActiveResources = resourcesResponse.Resources?.Count(r => r.IsActive) ?? 0;

            var model = new HomeSummaryViewModel
            {
                TotalResources = resourcesResponse.TotalCount,
                TotalReservations = reservationsResponse.TotalCount,
                TotalActiveResources = totalActiveResources,
                RecentReservations = reservationsResponse.Reservations?
                    .OrderByDescending(r => r.StartTime)
                    .Take(5)
                    .Select(r => new ReservationDetailsViewModel
                    {
                        ReservationId = r.ReservationId,
                        StartTime = r.StartTime,
                        EndTime = r.EndTime,
                    }).ToList() ?? new List<ReservationDetailsViewModel>(),
                RecentResources = resourcesResponse.Resources?
                    .OrderByDescending(r => r.ResourceId)
                    .Take(5)
                    .Select(r => new ResourceInfo
                    {
                        ResourceId = r.ResourceId,
                        Name = r.Name,
                        ResourceTypeId = r.ResourceTypeId,
                        Capacity = r.Capacity,
                        IsActive = r.IsActive
                    }).ToList() ?? new List<ResourceInfo>()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
