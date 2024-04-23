using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.FileMetaDataRepository
{
    public class FileMetaDataRepository : Repository<FileMetaData>, IFileMetaDataRepository
    {
        public FileMetaDataRepository(AppDBContext dbContext) : base(dbContext)
        {
        }
    }
}


