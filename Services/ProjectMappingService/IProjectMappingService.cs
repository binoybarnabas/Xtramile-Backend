using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.ProjectMappingService
{
    public interface IProjectMappingServices
    {
        public Task<IEnumerable<TBL_PROJECT_MAPPING>> GetProjectMappingsAsync();
        public Task AddProjectMappingAsync(TBL_PROJECT_MAPPING projectMapping);
    }
}
