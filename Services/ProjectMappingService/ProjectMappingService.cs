using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.ProjectMappingService
{
    public class ProjectMappingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectMappingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TBL_PROJECT_MAPPING>> GetProjectMappingAsync()
        {
            try
            {
                var projectMapping = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                return projectMapping;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting ProjectMapping: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task SetProjectMappingAsync(TBL_PROJECT_MAPPING projectMapping)
        {
            try
            {
                await _unitOfWork.ProjectMappingRepository.AddAsync(projectMapping);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while setting project mapping: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

    }
}
