using XtramileBackend.Models.EntityModels;
using XtramileBackend.Models.APIModels;


namespace XtramileBackend.Services.ProjectService
{
    public interface IProjectServices

    {
        public Task<IEnumerable<Project>> GetAllProjectsAsync();
        public Task AddProjectAsync(Project project);
        public Task<IEnumerable<object>> GetProjectIdAndCode();

        public Task<List<ProjectCodesViewModel>> GetProjectCodesByEmployeeId(int empId);


        public Task<string> GetProjectCodeByProjectIdAsync(int projectId);

    }
}

