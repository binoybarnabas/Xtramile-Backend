using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.ProjectRepository
{
    public class ProjectRepository:Repository<TBL_PROJECT>,IProjectRepository
    {
        public ProjectRepository(AppDBContext dbContext) : base(dbContext)
        {

        }
    }
}
