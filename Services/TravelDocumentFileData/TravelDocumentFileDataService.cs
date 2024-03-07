using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.TravelDocumentFileData
{
    public class TravelDocumentFileDataService : ITravelDocumentFileDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TravelDocumentFileDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<TravelDocumentFileDataModel>> GetTravelDocumentFileDatasAsync()
        {
            try
            {
                IEnumerable<TravelDocumentFileDataModel> travelDocumentFileData = await _unitOfWork.TravelDocumentFileDataRepository.GetAllAsync();
                return travelDocumentFileData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting travel document file data: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task<TravelDocumentFileDataModel> GetTravelDocumentFileByIdAsync(int id)
        {
            try
            {
                TravelDocumentFileDataModel travelDocumentFile = await _unitOfWork.TravelDocumentFileDataRepository.GetByIdAsync(id);
                return travelDocumentFile;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting travel document file data: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task AddTravelDocumentFileAsync(TravelDocumentFileDataModel TravelDocumentFileData)
        {
            try
            {
                await _unitOfWork.TravelDocumentFileDataRepository.AddAsync(TravelDocumentFileData);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding a travel document file : {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
