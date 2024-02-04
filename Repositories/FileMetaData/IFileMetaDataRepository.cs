using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.FileMetaDataRepository
{
    public interface IFileMetaDataRepository
    {
        Task AddAsync(TBL_FILE_METADATA fileMetaData);
        Task<IEnumerable<TBL_FILE_METADATA>> GetAllAsync();
    }
}