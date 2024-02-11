using Azure.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.ProjectService
{
    public class ProjectServices : IProjectServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<TBL_PROJECT>> GetAllProjectsAsync()
        {
            try
            {
                var projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                return projectData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting all projects: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task AddProjectAsync(TBL_PROJECT project)
        {
            try
            {
                project.CreatedOn = DateTime.Now;
                await _unitOfWork.ProjectRepository.AddAsync(project);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding a project: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task<List<ProjectCodesViewModel>?> GetProjectCodesByEmployeeId(int empId)
        {
            try
            {

            var projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
            var projectData =   await _unitOfWork.ProjectRepository.GetAllAsync();


             var result = (from projectMapping in projectMappingData
                              join project in projectData on projectMapping.ProjectId equals project.ProjectId 
                              where projectMapping.EmpId == empId
                              select new ProjectCodesViewModel
                              {
                                  ProjectId = project.ProjectId,
                                  ProjectCode = project.ProjectCode,
                                  ProjectName = project.ProjectName,

                              }).ToList();

                /*                return new List<ProjectCodesViewModel> { result };
                */
                return result;


            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting project codes: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
