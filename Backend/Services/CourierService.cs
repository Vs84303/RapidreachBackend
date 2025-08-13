using RapidReachNET.DTO;
using RapidReachNET.Models;
using RapidReachNET.Repositories;

namespace RapidReachNET.Services
{
    public class CourierService : ICourierService
    {
        private readonly ICourierRepository _courierRepository;
        private readonly IEmployeeRepository _userRepository;
        private readonly ITrackingRepository _trackingRepository;

        public CourierService(ICourierRepository courierRepository, IEmployeeRepository userRepository, ITrackingRepository trackingRepository)
        {
            _courierRepository = courierRepository;
            _userRepository = userRepository;
            _trackingRepository = trackingRepository;
        }

        public async Task<Courier> AddCourierAsync(CourierRequestDTO courierDto, long customerId)
        {
            var customer = await _userRepository.GetUserByIdAsync(customerId);


            // Create Courier entity
            var courier = new Courier
            {
                DropLocation = courierDto.DropLocation,
                ParcelDescription = courierDto.ParcelDescription,
                ParcelWeight = courierDto.ParcelWeight,
                PickUpLocation = courierDto.PickUpLocation,
                Price = courierDto.Price,
                BranchId = courierDto.BranchId,
                UserId = customerId
            };

            // Save courier first to get generated Id
            var addedCourier = await _courierRepository.AddCourierAsync(courier);

            // Create initial Tracking entry for this courier
            var tracking = new Tracking
            {
                CourierId = addedCourier.Id,
                CourierDate = DateOnly.FromDateTime(DateTime.Now),
                TrackingStatus = "Created"  // Or any initial status you want
            };

            await _trackingRepository.AddTrackingAsync(tracking);

            return addedCourier;
        }

        public async Task<IEnumerable<Courier>> GetAllCouriersAsync()
        {
            return await _courierRepository.GetAllCouriersAsync();
        }

        public async Task<Courier?> GetCourierByIdAsync(long id)
        {
            return await _courierRepository.GetCourierByIdAsync(id);
        }

        public async Task<IEnumerable<Courier>> GetCouriersByUserIdAsync(long userId)
        {
            return await _courierRepository.GetCouriersByUserIdAsync(userId);
        }

        public async Task<IEnumerable<CourierDto>> GetCouriersByBranchIdAsync(long branchId)
        {
            return await _courierRepository.GetCouriersByBranchIdAsync(branchId);
        }

    }
}
