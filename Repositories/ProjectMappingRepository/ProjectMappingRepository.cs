using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.ProjectMappingRepository
{
    public class ProjectMappingRepository : Repository<TBL_PROJECT_MAPPING>, IProjectMappingRepository
    {

        public ProjectMappingRepository(AppDBContext dbContext) : base(dbContext)
        {

        }

    }
}
