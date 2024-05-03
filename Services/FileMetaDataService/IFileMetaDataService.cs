using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.FileMetaDataService
{
    public interface IFileMetaDataService
    {
         public Task<IEnumerable<FileMetaData>> GetFileMetaDataAsync();
         public Task AddFileMetaDataAsync(FileMetaData fileMetaData);
        
         public Task<int> GetFileIdByFileNameAsync(string fileName);


        public Task<FileMetaData?> GetFilePathByRequestIdAndDescriptionAsync(int requestId, string description);


        public Task<string> GetFilePathByFileIdAsync(int fileId);

        public Task<FileMetaData> GetFileMetaDataById(int fileId);
        public Task<FileMetaData?> GetProfilePictureData(int empId);
        public Task<int> GetFileIdByRequestIdAndTravelAuthFile(int requestId);





    }
}
