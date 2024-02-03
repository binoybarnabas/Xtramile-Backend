using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.FileMetaDataService
{
    public class FileMetaDataService : IFileMetaDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FileMetaDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }


        public async Task AddFileMetaDataAsync(TBL_FILE_METADATA fileMetaData)
        {
            try
            {
                await _unitOfWork.FileMetaDataRepository.AddAsync(fileMetaData);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding an expense: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }


        public async Task<IEnumerable<TBL_FILE_METADATA>> GetFileMetaDataAsync()
        {
            try
            {
                var fileMetaData = await _unitOfWork.FileMetaDataRepository.GetAllAsync();
                return fileMetaData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting expenses: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
        

    }
}
