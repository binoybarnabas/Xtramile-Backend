using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.ProjectMappingService
{
    public class ProjectMappingServices : IProjectMappingServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectMappingServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<TBL_PROJECT_MAPPING>> GetProjectMappingsAsync()
        {
            try
            {
                var projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                return projectMappingData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting project mappings: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task AddProjectMappingAsync(TBL_PROJECT_MAPPING projectMapping)
        {
            try
            {
                await _unitOfWork.ProjectMappingRepository.AddAsync(projectMapping);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding a project mapping: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
