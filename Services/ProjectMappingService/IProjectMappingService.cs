using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.ProjectMappingService
{
    public interface IProjectMappingService
    {
        public Task<IEnumerable<TBL_PROJECT_MAPPING>> GetProjectMappingAsync();

        public Task SetProjectMappingAsync(TBL_PROJECT_MAPPING projectMapping);
    }
}
