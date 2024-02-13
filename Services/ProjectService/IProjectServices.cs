using XtramileBackend.Models.EntityModels;
using XtramileBackend.Models.APIModels;


namespace XtramileBackend.Services.ProjectService
{
    public interface IProjectServices

    {
        public Task<IEnumerable<TBL_PROJECT>> GetAllProjectsAsync();
        public Task AddProjectAsync(TBL_PROJECT project);
        public Task<IEnumerable<object>> GetProjectIdAndCode();

        public Task<List<ProjectCodesViewModel>> GetProjectCodesByEmployeeId(int empId);

    }
}

