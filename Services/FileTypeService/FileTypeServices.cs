using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.FileTypeService
{
    public class FileTypeServices : IFileTypeServices
    {

        private readonly IUnitOfWork _unitOfWork;
        public FileTypeServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<IEnumerable<TBL_FILE_TYPE>> GetFileTypesAsync()
        {
            try
            {
                var fileTypes = _unitOfWork.FileTypeRepository.GetAllAsync();
                return fileTypes;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting file type: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }

        }

        public async Task AddFileTypeAsync(TBL_FILE_TYPE files)
        {
            try
            {
                await _unitOfWork.FileTypeRepository.AddAsync(files);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding file type: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
