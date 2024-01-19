using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.FileTypeRepository
{
    public class FileTypeRepository : Repository<TBL_FILE_TYPE>, IFileTypeRepository
    {

        public FileTypeRepository(AppDBContext dbContext) : base(dbContext)
        {

        }

    }
}
