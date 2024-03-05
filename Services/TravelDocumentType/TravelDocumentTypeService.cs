using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.TravelDocumentType
{
    public class TravelDocumentTypeService: ITravelDocumentTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TravelDocumentTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<TravelDocumentTypeModel>> GetTravelDocumentTypesAsync()
        {
            try
            {
                var travelDocumentTypeData = await _unitOfWork.TravelDocumentTypeRepository.GetAllAsync();
                return travelDocumentTypeData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting travel document types: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task AddTravelDocumentTypeAsync(TravelDocumentTypeModel TravelDocumentType)
        {
            try
            {
                await _unitOfWork.TravelDocumentTypeRepository.AddAsync(TravelDocumentType);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding a travel document types: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
