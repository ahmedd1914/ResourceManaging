using ResourceManaging.Services.Interfaces.Reservation;
using ResourceManaging.Services.DTOs.Reservation;
using ResourceManaging.Repository.Interfaces.Reservation;
using ResourceManaging.Models;

namespace ResourceManaging.Services.Implementations.Reservation
{
    public class ReservationResourceService : IReservationResourceService
    {
        private readonly IReservationResourceRepository _reservationResourceRepository;

        public ReservationResourceService(IReservationResourceRepository reservationResourceRepository)
        {
            _reservationResourceRepository = reservationResourceRepository;
        }

        public async Task<ReservationResourceResponse> GetReservationResourceByIdAsync(int reservationId, int resourceId)
        {
            var filter = new ReservationResourceFilter();
            filter.AddReservationFilter(reservationId);
            filter.AddResourceFilter(resourceId);

            var resources = await _reservationResourceRepository.RetrieveByFilterAsync(filter);
            var resource = resources.FirstOrDefault();

            if (resource == null)
            {
                return new ReservationResourceResponse
                {
                    Success = false,
                    Message = "Reservation resource not found"
                };
            }

            return new ReservationResourceResponse
            {
                Success = true,
                Message = "Reservation resource retrieved successfully",
                ReservationResource = ReservationResourceData.FromReservationResource(resource)
            };
        }

        public async Task<ReservationResourceListResponse> GetReservationResourcesByFilterAsync(ReservationResourceFilter filter)
        {
            var resources = await _reservationResourceRepository.RetrieveByFilterAsync(filter);
            return new ReservationResourceListResponse
            {
                Success = true,
                Message = "Reservation resources retrieved successfully",
                ReservationResources = resources.Select(ReservationResourceData.FromReservationResource).ToList(),
                TotalCount = resources.Count()
            };
        }

        public async Task<ReservationResourceResponse> CreateReservationResourceAsync(CreateReservationResourceRequest request)
        {
            var resource = new ReservationResource
            {
                ReservationId = request.ReservationId,
                ResourceId = request.ResourceId
            };

            var id = await _reservationResourceRepository.CreateAsync(resource);
            if (id <= 0)
            {
                return new ReservationResourceResponse
                {
                    Success = false,
                    Message = "Failed to create reservation resource"
                };
            }

            return new ReservationResourceResponse
            {
                Success = true,
                Message = "Reservation resource created successfully",
                ReservationResource = ReservationResourceData.FromReservationResource(resource)
            };
        }

        public async Task<bool> UpdateReservationResourceAsync(UpdateReservationResourceRequest request)
        {
            var resource = await _reservationResourceRepository.RetrieveByIdAsync(request.ReservationId);
            if (resource == null)
            {
                return false;
            }

            var update = new ReservationResourceUpdate();
            update.UpdateResourceId(request.ResourceId);

            return await _reservationResourceRepository.UpdateAsync(update);
        }

        public async Task<bool> DeleteReservationResourceAsync(int reservationId, int resourceId)
        {
            return await _reservationResourceRepository.DeleteAsync(reservationId);
        }

        public async Task<ReservationResourceListResponse> GetResourcesByReservationIdAsync(int reservationId)
        {
            var filter = new ReservationResourceFilter();
            filter.AddReservationFilter(reservationId);
            return await GetReservationResourcesByFilterAsync(filter);
        }

        public async Task<ReservationResourceListResponse> GetReservationsByResourceIdAsync(int resourceId)
        {
            var filter = new ReservationResourceFilter();
            filter.AddResourceFilter(resourceId);
            return await GetReservationResourcesByFilterAsync(filter);
        }

        public async Task<ReservationResourceResponse> AddResourceToReservationAsync(AddResourceToReservationRequest request)
        {
            return await CreateReservationResourceAsync(new CreateReservationResourceRequest
            {
                ReservationId = request.ReservationId,
                ResourceId = request.ResourceId
            });
        }

        public async Task<bool> RemoveResourceFromReservationAsync(RemoveResourceFromReservationRequest request)
        {
            return await DeleteReservationResourceAsync(request.ReservationId, request.ResourceId);
        }
    }
}
