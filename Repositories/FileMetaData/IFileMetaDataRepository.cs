using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.FileMetaDataRepository
{
    public interface IFileMetaDataRepository
    {
        Task AddAsync(FileMetaData fileMetaData);
        Task<IEnumerable<FileMetaData>> GetAllAsync();
        Task<FileMetaData> GetByIdAsync(int FileId);
    }
}