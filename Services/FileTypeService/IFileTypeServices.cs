using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.FileTypeService
{
    public interface IFileTypeServices
    {
        public Task<IEnumerable<TBL_FILE_TYPE>> GetFileTypesAsync();
        public Task AddFileTypeAsync(TBL_FILE_TYPE files);

        public Task<int> GetFileTypeIdByExtensionAsync(string fileExtension);


    }
}
