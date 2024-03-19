using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.FileMetaDataService
{
    public interface IFileMetaDataService
    {
         public Task<IEnumerable<TBL_FILE_METADATA>> GetFileMetaDataAsync();
         public Task AddFileMetaDataAsync(TBL_FILE_METADATA fileMetaData);
        
         public Task<int> GetFileIdByFileNameAsync(string fileName);


        public Task<TBL_FILE_METADATA?> GetFilePathByRequestIdAndDescriptionAsync(int requestId, string description);


        public Task<string> GetFilePathByFileIdAsync(int fileId);

        public Task<TBL_FILE_METADATA> GetFileMetaDataById(int fileId);
 

    }
}
