using XtramileBackend.Models.EntityModels;
using XtramileBackend.Models.APIModels;


namespace XtramileBackend.Services.ProjectService
{
    public interface IProjectServices

    {
        public Task<IEnumerable<TBL_PROJECT>> GetAllProjectsAsync();
        public Task AddProjectAsync(TBL_PROJECT project);

        public Task<List<ProjectCodesViewModel>> GetProjectCodesByEmployeeId(int empId);


        public Task<string> GetProjectCodeByProjectIdAsync(int projectId);

    }
}

