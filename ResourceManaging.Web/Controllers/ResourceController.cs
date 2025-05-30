using Microsoft.AspNetCore.Mvc;
using ResourceManaging.Repository.Interfaces.Resource;
using ResourceManaging.Services.DTOs.Resource;
using ResourceManaging.Services.Interfaces.Resource;
using ResourceManaging.Web.Models.ViewModels.Resource;
using System.Linq;
using ResourceManaging.Web.Attributes;
using ResourceManaging.Web.Models;

namespace ResourceManaging.Web.Controllers
{
    [Authorize]
    public class ResourceController : Controller
    {
        private readonly IResourceService _resourceService;
        private readonly IResourceTypeService _resourceTypeService;

        public ResourceController(IResourceService resourceService, IResourceTypeService resourceTypeService)
        {
            _resourceService = resourceService;
            _resourceTypeService = resourceTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var resources = await _resourceService.GetResourcesByFilterAsync(new ResourceFilter());
            var model = new ResourceListViewModel
            {
                Resources = resources.Resources?.Select(r => new ResourceInfo
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

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var resource = await _resourceService.GetResourceByIdAsync(id);
            if (!resource.Success) return NotFound();

            var types = await _resourceTypeService.GetResourceTypesByFilterAsync(new ResourceTypeFilter());
            var type = types.ResourceTypes.FirstOrDefault(t => t.ResourceTypeId == resource.Resource.ResourceTypeId);

            var model = new ResourceDetailsViewModel
            {
                ResourceId = resource.Resource.ResourceId,
                Name = resource.Resource.Name,
                ResourceType = new ResourceTypeInfo
                {
                    ResourceTypeId = resource.Resource.ResourceTypeId,
                    Name = type?.Name
                },
                Capacity = resource.Resource.Capacity,
                IsActive = resource.Resource.IsActive
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var types = await _resourceTypeService.GetResourceTypesByFilterAsync(new ResourceTypeFilter());
            var model = new CreateResourceViewModel
            {
                ResourceTypes = types.ResourceTypes?.Select(t => new ResourceTypeInfo
                {
                    ResourceTypeId = t.ResourceTypeId,
                    Name = t.Name
                }).ToList() ?? new List<ResourceTypeInfo>()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateResourceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var types = await _resourceTypeService.GetResourceTypesByFilterAsync(new ResourceTypeFilter());
                model.ResourceTypes = types.ResourceTypes?.Select(t => new ResourceTypeInfo
                {
                    ResourceTypeId = t.ResourceTypeId,
                    Name = t.Name
                }).ToList() ?? new List<ResourceTypeInfo>();
                return View(model);
            }

            var request = new CreateResourceRequest
            {
                Name = model.Name,
                ResourceTypeId = model.ResourceTypeId,
                Capacity = model.Capacity
            };
            var response = await _resourceService.CreateResourceAsync(request);
            if (response.Success) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", response.Message);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var resourceResponse = await _resourceService.GetResourceByIdAsync(id);
            if (!resourceResponse.Success) return NotFound();

            var types = await _resourceTypeService.GetResourceTypesByFilterAsync(new ResourceTypeFilter());

            var model = new EditResourceViewModel
            {
                ResourceId = resourceResponse.Resource.ResourceId,
                Name = resourceResponse.Resource.Name,
                ResourceTypeId = resourceResponse.Resource.ResourceTypeId,
                Capacity = resourceResponse.Resource.Capacity,
                IsActive = resourceResponse.Resource.IsActive,
                ResourceTypes = types.ResourceTypes?.Select(t => new ResourceTypeInfo
                {
                    ResourceTypeId = t.ResourceTypeId,
                    Name = t.Name
                }).ToList() ?? new List<ResourceTypeInfo>()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditResourceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var types = await _resourceTypeService.GetResourceTypesByFilterAsync(new ResourceTypeFilter());
                model.ResourceTypes = types.ResourceTypes?.Select(t => new ResourceTypeInfo
                {
                    ResourceTypeId = t.ResourceTypeId,
                    Name = t.Name
                }).ToList() ?? new List<ResourceTypeInfo>();
                return View(model);
            }

            var request = new UpdateResourceRequest
            {
                ResourceId = model.ResourceId,
                Name = model.Name,
                ResourceTypeId = model.ResourceTypeId,
                Capacity = model.Capacity,
                IsActive = model.IsActive
            };
            var success = await _resourceService.UpdateResourceAsync(request);
            if (success) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Failed to update resource");
            return View(model);
        }
    }
}
