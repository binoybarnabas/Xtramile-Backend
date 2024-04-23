using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.ProjectMappingService
{
    public interface IProjectMappingServices
    {
        public Task<IEnumerable<ProjectEmployeeMap>> GetProjectMappingsAsync();
        public Task AddProjectMappingAsync(ProjectEmployeeMap projectMapping);
    }
}
