using Microsoft.AspNetCore.Mvc;
using ResourceManaging.Repository.Interfaces.Resource;
using ResourceManaging.Services.DTOs.Resource;
using ResourceManaging.Services.Interfaces.Resource;
using ResourceManaging.Web.Models;
using ResourceManaging.Web.Models.ViewModels.Resource;
using System.Linq;
using ResourceManaging.Web.Attributes;
namespace ResourceManaging.Web.Controllers
{
    [Authorize]
    public class ResourceTypeController : Controller
    {
        private readonly IResourceTypeService _resourceTypeService;

        public ResourceTypeController(IResourceTypeService resourceTypeService)
        {
            _resourceTypeService = resourceTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var types = await _resourceTypeService.GetResourceTypesByFilterAsync(new ResourceTypeFilter());
            var model = new ResourceTypeListViewModel
            {
                ResourceTypes = types.ResourceTypes?.Select(t => new ResourceTypeInfo
                {
                    ResourceTypeId = t.ResourceTypeId,
                    Name = t.Name,
                    Description = t.Description
                }).ToList() ?? new List<ResourceTypeInfo>()
            };
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var response = await _resourceTypeService.GetResourceTypeByIdAsync(id);
            if (!response.Success) return NotFound();
            var model = new ResourceTypeDetailsViewModel
            {
                ResourceTypeId = response.ResourceType.ResourceTypeId,
                Name = response.ResourceType.Name,
                Description = response.ResourceType.Description
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateResourceTypeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateResourceTypeViewModel model)
        {
            if(!ModelState.IsValid) return View(model);
            var request = new CreateResourceTypeRequest
            {
                Name = model.Name,
                Description = model.Description
            };
            var response = await _resourceTypeService.CreateResourceTypeAsync(request);
            if(response.Success) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", response.Message);
            return View(model);
        }   

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _resourceTypeService.GetResourceTypeByIdAsync(id);
            if(!response.Success) return NotFound();

            var model = new EditResourceTypeViewModel
            {
                ResourceTypeId = response.ResourceType.ResourceTypeId,
                Name = response.ResourceType.Name,
                Description = response.ResourceType.Description
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditResourceTypeViewModel model)
        {
            if(!ModelState.IsValid) return View(model);
            var request = new UpdateResourceTypeRequest
            {
                ResourceTypeId = model.ResourceTypeId,
                Name = model.Name,
                Description = model.Description
            };
            var success = await _resourceTypeService.UpdateResourceTypeAsync(request);
            if(success) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Failed to update resource type");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _resourceTypeService.DeleteResourceTypeAsync(id);
            if(success) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Failed to delete resource type");
            return RedirectToAction(nameof(Index));
        }
    }
}
