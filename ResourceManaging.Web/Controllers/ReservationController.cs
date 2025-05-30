using Microsoft.AspNetCore.Mvc;
using ResourceManaging.Repository.Interfaces.Reservation;
using ResourceManaging.Services.DTOs.Reservation;
using ResourceManaging.Services.Interfaces.Reservation;
using ResourceManaging.Web.Models;
using ResourceManaging.Services.Interfaces.Resource;
using System.Linq;
using ResourceManaging.Web.Models.ViewModels.Resource;
using ResourceManaging.Repository.Interfaces.Resource;
using ResourceManaging.Web.Attributes;

namespace ResourceManaging.Web.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IResourceService _resourceService;

        public ReservationController(
            IReservationService reservationService,
            IResourceService resourceService)
        {
            _reservationService = reservationService;
            _resourceService = resourceService;
        }

        private async Task<List<ResourceDetailsViewModel>> GetAvailableResourcesAsync()
        {
            var response = await _resourceService.GetResourcesByFilterAsync(new ResourceFilter { IsActive = true });
            return response.Resources?.Select(r => new ResourceDetailsViewModel
            {
                ResourceId = r.ResourceId,
                Name = r.Name,
                ResourceTypeId = r.ResourceTypeId,
                Capacity = r.Capacity,
                IsActive = r.IsActive
            }).ToList() ?? new List<ResourceDetailsViewModel>();
        }

        public async Task<IActionResult> Index()
        {
            var response = await _reservationService.GetReservationsByFilterAsync(new ReservationFilter());
            var model = new ReservationListViewModel
            {
                Reservations = response.Reservations?
                    .Where(r => !r.IsCancelled)
                    .Select(r => new ReservationDetailsViewModel
                    {
                        ReservationId = r.ReservationId,
                        StartTime = r.StartTime,
                        EndTime = r.EndTime,
                        Purpose = r.Purpose,
                        Participants = r.Participants,
                        IsCancelled = r.IsCancelled,
                        CreatedAt = r.CreatedAt,
                        ResourceIds = r.ResourceIds
                    }).ToList() ?? new List<ReservationDetailsViewModel>()
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _reservationService.GetReservationByIdAsync(id);
            if (!response.Success) return NotFound();

            // Get resource details for each resource ID
            var resourceIds = response.Reservation.ResourceIds;
            var resources = new List<ResourceDetailsViewModel>();
            foreach (var resourceId in resourceIds)
            {
                var resourceResponse = await _resourceService.GetResourceByIdAsync(resourceId);
                if (resourceResponse.Success)
                {
                    resources.Add(new ResourceDetailsViewModel
                    {
                        ResourceId = resourceResponse.Resource.ResourceId,
                        Name = resourceResponse.Resource.Name,
                        ResourceTypeId = resourceResponse.Resource.ResourceTypeId,
                        Capacity = resourceResponse.Resource.Capacity,
                        IsActive = resourceResponse.Resource.IsActive
                    });
                }
            }

            var model = new ReservationDetailsViewModel
            {
                ReservationId = response.Reservation.ReservationId,
                ReservorId = response.Reservation.ReservorId,
                StartTime = response.Reservation.StartTime,
                EndTime = response.Reservation.EndTime,
                Purpose = response.Reservation.Purpose,
                Participants = response.Reservation.Participants,
                IsCancelled = response.Reservation.IsCancelled,
                CreatedAt = response.Reservation.CreatedAt,
                ResourceIds = resourceIds,
                Resources = resources
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateReservationViewModel
            {
                AvailableResources = await GetAvailableResourcesAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableResources = await GetAvailableResourcesAsync();
                return View(model);
            }

            var reservorId = HttpContext.Session.GetInt32("UserId");
            if (reservorId == null)
            {
                ModelState.AddModelError("", "User not found");
                model.AvailableResources = await GetAvailableResourcesAsync();
                return View(model);
            }

            if (model.SelectedResourceIds != null && model.SelectedResourceIds.Any())
            {
                var totalCapacity = 0;
                foreach (var resourceId in model.SelectedResourceIds)
                {
                    var resourceResponse = await _resourceService.GetResourceByIdAsync(resourceId);
                    if (resourceResponse.Success)
                    {
                        totalCapacity += resourceResponse.Resource.Capacity;
                    }
                }

                if (model.Participants > totalCapacity)
                {
                    ModelState.AddModelError("Participants", $"Total capacity of selected resources ({totalCapacity}) is less than number of participants ({model.Participants})");
                    model.AvailableResources = await GetAvailableResourcesAsync();
                    return View(model);
                }
            }

            var request = new CreateReservationRequest
            {
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Purpose = model.Purpose,
                Participants = model.Participants,
                ResourceIds = model.SelectedResourceIds ?? new List<int>(),
                ReservorId = reservorId.Value
            };

            var response = await _reservationService.CreateReservationAsync(request);
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            model.AvailableResources = await GetAvailableResourcesAsync();
            ViewData["ErrorMessage"] = response.Message;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _reservationService.GetReservationByIdAsync(id);
            if (!response.Success) return NotFound();

            var model = new EditReservationViewModel
            {
                ReservationId = response.Reservation.ReservationId,
                StartTime = response.Reservation.StartTime,
                EndTime = response.Reservation.EndTime,
                Purpose = response.Reservation.Purpose,
                Participants = response.Reservation.Participants,
                IsCancelled = response.Reservation.IsCancelled,
                SelectedResourceIds = response.Reservation.ResourceIds,
                AvailableResources = await GetAvailableResourcesAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableResources = await GetAvailableResourcesAsync();
                return View(model);
            }

            if (model.SelectedResourceIds != null && model.SelectedResourceIds.Any())
            {
                var totalCapacity = 0;
                foreach (var resourceId in model.SelectedResourceIds)
                {
                    var resourceResponse = await _resourceService.GetResourceByIdAsync(resourceId);
                    if (resourceResponse.Success)
                    {
                        totalCapacity += resourceResponse.Resource.Capacity;
                    }
                }

                if (model.Participants > totalCapacity)
                {
                    ModelState.AddModelError("Participants", $"Total capacity of selected resources ({totalCapacity}) is less than number of participants ({model.Participants})");
                    model.AvailableResources = await GetAvailableResourcesAsync();
                    return View(model);
                }
            }

            var request = new UpdateReservationRequest
            {
                ReservationId = model.ReservationId,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Purpose = model.Purpose,
                Participants = model.Participants,
                IsCancelled = model.IsCancelled,
                ResourceIds = model.SelectedResourceIds ?? new List<int>()
            };

            var (success, message) = await _reservationService.UpdateReservationAsync(request);
            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction(nameof(Index));
            }
            
            model.AvailableResources = await GetAvailableResourcesAsync();
            ViewData["ErrorMessage"] = message;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _reservationService.DeleteReservationAsync(id);
            if (success) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Delete failed.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ReleaseEarly(int id)
        {
            var success = await _reservationService.ReleaseReservationEarlyAsync(id);
            if (success) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Failed to release reservation early.");
            return RedirectToAction(nameof(Index));
        }
    }
}
