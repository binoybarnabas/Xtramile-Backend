using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.TravelTypeService
{
    public class TravelTypeService : ITravelTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TravelTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TBL_TRAVEL_TYPE>> GetTravelTypeAsync()
        {
            try
            {
                var travelData = await _unitOfWork.TravelTypeRepository.GetAllAsync();
                return travelData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting Travel Type: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task SetTravelTypeAsync(TBL_TRAVEL_TYPE travelType)
        {
            try
            {
                await _unitOfWork.TravelTypeRepository.AddAsync(travelType);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while setting Travel Type: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

    }
}
