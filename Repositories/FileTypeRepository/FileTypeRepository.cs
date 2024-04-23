using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.FileTypeRepository
{
    public class FileTypeRepository : Repository<FileFormat>, IFileTypeRepository
    {

        public FileTypeRepository(AppDBContext dbContext) : base(dbContext)
        {

        }

    }
}
