using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.FileTypeService
{
    public interface IFileTypeServices
    {
        public Task<IEnumerable<FileFormat>> GetFileTypesAsync();
        public Task AddFileTypeAsync(FileFormat files);
        public Task<int> GetFileTypeIdByExtensionAsync(string fileExtension);

    }
}
