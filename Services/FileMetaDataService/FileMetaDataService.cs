using Azure.Core;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.FileMetaDataService
{
    public class FileMetaDataService : IFileMetaDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FileMetaDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }


        public async Task AddFileMetaDataAsync(TBL_FILE_METADATA fileMetaData)
        {
            try
            {
                await _unitOfWork.FileMetaDataRepository.AddAsync(fileMetaData);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding file meta data: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        //implement 
        public async Task<int> GetFileIdByFileNameAsync(string fileName)
        {
            try
            {
                IEnumerable<TBL_FILE_METADATA> fileMetaData = await _unitOfWork.FileMetaDataRepository.GetAllAsync();

                var fileId = (from item in fileMetaData
                                 where item.FileName == fileName
                                 select item.FileId).FirstOrDefault();

                return fileId;

            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting file id: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }

            //EOF
         }

            public async Task<IEnumerable<TBL_FILE_METADATA>> GetFileMetaDataAsync()
        {
            try
            {
                var fileMetaData = await _unitOfWork.FileMetaDataRepository.GetAllAsync();
                return fileMetaData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting file metadata: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

 
        //Get File Path RequestId and description
        public async Task<TBL_FILE_METADATA> GetFilePathByRequestIdAndDescriptionAsync(int requestId, string description)
        {
            try
            {
                var fileMetaData = await _unitOfWork.FileMetaDataRepository.GetAllAsync();

                var fileData = (from item in fileMetaData
                                where item.RequestId == requestId && item.Description == description
                                select item).FirstOrDefault();

                return fileData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting file path: {ex.Message}");
                throw;
            }
        }

        //Get FIle Path by file id
        public async Task<string> GetFilePathByFileIdAsync(int fileId)
        {
            try
            {
                var fileMetaData = await _unitOfWork.FileMetaDataRepository.GetAllAsync();

                var filePath = (from item in fileMetaData
                                where item.FileId == fileId
                                select item.FilePath).FirstOrDefault();

                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting file path: {ex.Message}");
                throw;
            }
        }

        public async Task<TBL_FILE_METADATA> GetFileMetaDataById(int Fileid)
        {
            try
            {
                TBL_FILE_METADATA fileData = await _unitOfWork.FileMetaDataRepository.GetByIdAsync(Fileid);
                return fileData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting FileData: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }



    }
}
