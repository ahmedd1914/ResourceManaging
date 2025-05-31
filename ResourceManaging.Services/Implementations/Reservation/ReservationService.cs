using ResourceManaging.Services.Interfaces.Reservation;
using ResourceManaging.Services.DTOs.Reservation;
using ResourceManaging.Repository.Interfaces.Reservation;
using ResourceManaging.Models;
using ResourceManaging.Repository.Interfaces.Resource;

namespace ResourceManaging.Services.Implementations.Reservation
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IResourceRepository _resourceRepository;

        public ReservationService(
            IReservationRepository reservationRepository,
            IResourceRepository resourceRepository)
        {
            _reservationRepository = reservationRepository;
            _resourceRepository = resourceRepository;
        }

        // Helper to get resource IDs for a reservation
        private async Task<List<int>> GetResourceIdsForReservationAsync(int reservationId)
        {
            return await _reservationRepository.GetResourceIdsForReservationAsync(reservationId);
        }

        public async Task<ReservationResponse> GetReservationByIdAsync(int id)
        {
            var reservation = await _reservationRepository.RetrieveByIdAsync(id);
            if (reservation == null)
            {
                return new ReservationResponse
                {
                    Success = false,
                    Message = "Reservation not found"
                };
            }
            var resourceIds = await GetResourceIdsForReservationAsync(reservation.ReservationId);
            return new ReservationResponse
            {
                Success = true,
                Message = "Reservation retrieved successfully",
                Reservation = ReservationData.FromReservation(reservation, resourceIds)
            };
        }

        public async Task<ReservationListResponse> GetReservationsByFilterAsync(ReservationFilter filter)
        {
            var reservations = await _reservationRepository.RetrieveByFilterAsync(filter);
            var reservationList = new List<ReservationData>();
            foreach (var reservation in reservations)
            {
                var resourceIds = await GetResourceIdsForReservationAsync(reservation.ReservationId);
                reservationList.Add(ReservationData.FromReservation(reservation, resourceIds));
            }
            return new ReservationListResponse
            {
                Success = true,
                Message = "Reservations retrieved successfully",
                Reservations = reservationList,
                TotalCount = reservationList.Count
            };
        }

        public async Task<ReservationResponse> CreateReservationAsync(CreateReservationRequest request)
        {
       var now = DateTime.UtcNow;

    if (request.StartTime <= now)
    {
        return new ReservationResponse
        {
            Success = false,
            Message = "Start time must be in the future."
        };
    }

    if (request.EndTime <= request.StartTime)
    {
        return new ReservationResponse
        {
            Success = false,
            Message = "End time must be after the start time."
        };
    }
            // Validate that all requested resources exist and are active
            foreach (var resourceId in request.ResourceIds)
            {
                var resource = await _resourceRepository.RetrieveByIdAsync(resourceId);
                if (resource == null || !resource.IsActive)
                {
                    return new ReservationResponse
                    {
                        Success = false,
                        Message = "One or more selected resources are not available. Please choose from the available list."
                    };
                }
            }

            // Check for conflicts with any of the requested resources
            var conflictFilter = new ReservationFilter();
            var existing = await _reservationRepository.RetrieveByFilterAsync(conflictFilter);
            foreach (var resourceId in request.ResourceIds)
            {
                foreach (var r in existing)
                {
                    var rResourceIds = await GetResourceIdsForReservationAsync(r.ReservationId);
                    if (r.StartTime < request.EndTime && request.StartTime < r.EndTime && rResourceIds.Contains(resourceId))
                    {
                        return new ReservationResponse
                        {
                            Success = false,
                            Message = "One or more selected resources are already reserved for the selected period. Please choose a different time or resource."
                        };
                    }
                }
            }

            var reservation = new Models.Reservation
            {
                ReservorId = request.ReservorId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Purpose = request.Purpose,
                Participants = request.Participants,
                IsCancelled = false,
                CreatedAt = DateTime.UtcNow
            };

            // You must add a method to your repository to accept resourceIds
            var id = await _reservationRepository.CreateAsync(reservation, request.ResourceIds);
            if (id <= 0)
            {
                return new ReservationResponse
                {
                    Success = false,
                    Message = "Failed to create reservation"
                };
            }

            reservation.ReservationId = id;
            return new ReservationResponse
            {
                Success = true,
                Message = "Reservation created successfully",
                Reservation = ReservationData.FromReservation(reservation, request.ResourceIds)
            };
        }

        public async Task<(bool Success, string Message)> UpdateReservationAsync(UpdateReservationRequest request)
        {
            // Prevent updates to past periods
            if (request.StartTime < DateTime.Now || request.EndTime <= request.StartTime)
            {
                return (false, "Start time must be in the future and end time must be after start time.");
            }

            // Validate that all requested resources exist and are active
            foreach (var resourceId in request.ResourceIds)
            {
                var resource = await _resourceRepository.RetrieveByIdAsync(resourceId);
                if (resource == null || !resource.IsActive)
                {
                    return (false, "One or more selected resources are not available. Please choose from the available list.");
                }
            }

            // Check for conflicts with any of the requested resources
            var conflictFilter = new ReservationFilter();
            var existing = await _reservationRepository.RetrieveByFilterAsync(conflictFilter);
            foreach (var resourceId in request.ResourceIds)
            {
                foreach (var r in existing)
                {
                    if (r.ReservationId != request.ReservationId)
                    {
                        var rResourceIds = await GetResourceIdsForReservationAsync(r.ReservationId);
                        if (r.StartTime < request.EndTime && request.StartTime < r.EndTime && rResourceIds.Contains(resourceId))
                        {
                            return (false, "One or more selected resources are already reserved for the selected period. Please choose a different time or resource.");
                        }
                    }
                }
            }

            var reservation = await _reservationRepository.RetrieveByIdAsync(request.ReservationId);
            if (reservation == null)
            {
                return (false, "Reservation not found.");
            }

            var update = new Repository.Interfaces.Reservation.ReservationUpdate
            {
                ReservationId = request.ReservationId
            };

            // Always set these properties to ensure they're included in the update
            update.UpdateStartTime(request.StartTime);
            update.UpdateEndTime(request.EndTime);
            update.UpdatePurpose(request.Purpose);
            update.UpdateParticipants(request.Participants);
            update.UpdateCancelledStatus(request.IsCancelled);
            update.UpdateResourceIds(request.ResourceIds);

            var success = await _reservationRepository.UpdateAsync(update);
            return (success, success ? "Reservation updated successfully." : "Failed to update reservation.");
        }

        public async Task<bool> DeleteReservationAsync(int id)
        {
            return await _reservationRepository.DeleteAsync(id);
        }

        public async Task<bool> ReleaseReservationEarlyAsync(int reservationId)
        {
            var reservation = await _reservationRepository.RetrieveByIdAsync(reservationId);
            if (reservation == null || reservation.EndTime <= DateTime.Now)
                return false;

            var update = new Repository.Interfaces.Reservation.ReservationUpdate
            {
                ReservationId = reservationId
            };
            update.UpdateEndTime(DateTime.Now);
            update.UpdateCancelledStatus(true);
            return await _reservationRepository.UpdateAsync(update);
        }
    }
}
