using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.ProjectRepository
{
    public class ProjectRepository:Repository<Project>,IProjectRepository
    {
        public ProjectRepository(AppDBContext dbContext) : base(dbContext)
        {

        }
    }
}
