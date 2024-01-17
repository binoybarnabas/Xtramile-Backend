using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.ProjectService
{
    public interface IProjectServices

    {
        public Task<IEnumerable<TBL_PROJECT>> GetAllProjectsAsync();
        public Task AddProjectAsync(TBL_PROJECT project);

    }
}

