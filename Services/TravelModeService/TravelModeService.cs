using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.TravelModeService
{
    public class TravelModeService: ITravelModeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TravelModeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TBL_TRAVEL_MODE>> GetTravelModeAsync()
        {
            try
            {
                var travelData = await _unitOfWork.TravelModeRepository.GetAllAsync();
                return travelData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting TravelMode: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task SetTravelModeAsync(TBL_TRAVEL_MODE travelMode)
        {
            try
            {
                await _unitOfWork.TravelModeRepository.AddAsync(travelMode);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
               // Handle or log the exception
                Console.WriteLine($"An error occurred while setting Travel Mode: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
