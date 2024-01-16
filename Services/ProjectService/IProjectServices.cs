using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.ProjectService
{
    public interface IProjectServices

    { 
        public IEnumerable<TBL_PROJECT> GetAllProjects();
        public void AddProject(TBL_PROJECT project);

    }
}

