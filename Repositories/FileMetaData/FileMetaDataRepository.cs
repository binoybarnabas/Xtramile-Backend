using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.FileMetaDataRepository
{
    public class FileMetaDataRepository : Repository<TBL_FILE_METADATA>, IFileMetaDataRepository
    {
        public FileMetaDataRepository(AppDBContext dbContext) : base(dbContext)
        {
        }
    }
}


