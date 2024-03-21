using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.FileTypeService;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.TravelDocumentFileData
{
    public class TravelDocumentFileDataService : ITravelDocumentFileDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileTypeServices _fileTypeServices;

        public TravelDocumentFileDataService(IUnitOfWork unitOfWork, IFileTypeServices fileTypeServices)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _fileTypeServices = fileTypeServices;
        }

        public async Task<IEnumerable<TravelDocumentFileDataModel>> GetTravelDocumentFileDatasAsync()
        {
            try
            {
                IEnumerable<TravelDocumentFileDataModel> travelDocumentFileData = await _unitOfWork.TravelDocumentFileDataRepository.GetAllAsync();
                return travelDocumentFileData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting travel document file data: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task<TravelDocumentFileDataModel> GetTravelDocumentFileByIdAsync(int id)
        {
            try
            {
                TravelDocumentFileDataModel travelDocumentFile = await _unitOfWork.TravelDocumentFileDataRepository.GetByIdAsync(id);
                return travelDocumentFile;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting travel document file data: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        /// <summary>
        /// Function to add a travel document uploaded by an employee to the database
        /// </summary>
        /// <param name="travelDocFile"></param>
        /// <param name="httpContext"></param>
        /// <returns>The uploaded file metadata</returns>
        public async Task<TravelDocumentFileDataModel> AddTravelDocumentFileAsync(TravelDocument travelDocFile, HttpContext httpContext)
        {
            try
            {
                string uploadsDirectory = "";
                string fileName = "";
                string filePath = "";
                int fileTypeId = 0;

                if (string.Compare(travelDocFile.TravelDocType, "ID Card") == 0)
                    uploadsDirectory = "Uploads/TravelDocuments/IdCards";
                else if (string.Compare(travelDocFile.TravelDocType, "Passport") == 0)
                    uploadsDirectory = "Uploads/TravelDocuments/Passports";
                else if (string.Compare(travelDocFile.TravelDocType, "Visa") == 0)
                    uploadsDirectory = "Uploads/TravelDocuments/Visas";

                if (!Directory.Exists(uploadsDirectory))
                {
                    // Create directory
                    try
                    {
                        Directory.CreateDirectory(uploadsDirectory);
                        Console.WriteLine("Directory created successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating directory: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Directory already exists.");
                }

                if (httpContext.Request.Form.Files != null)
                {
                    var file = httpContext.Request.Form.Files[0];
                    fileName = $"{travelDocFile.TravelDocType}_{travelDocFile.UploadedBy}_{file.FileName}";
                    filePath = Path.Combine(uploadsDirectory, fileName).Replace("\\", "/");
                    string fileExtension = Path.GetExtension(filePath);
                    fileTypeId = await _fileTypeServices.GetFileTypeIdByExtensionAsync(fileExtension.Substring(1));
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                TravelDocumentFileDataModel travelDocuments = new TravelDocumentFileDataModel
                {
                    TravelDocType = travelDocFile.TravelDocType,
                    FileName = fileName,
                    FilePath = uploadsDirectory,
                    FileTypeId = fileTypeId,
                    UploadedBy = travelDocFile.UploadedBy,
                    Country = travelDocFile.Country,
                    ExpiryDate = travelDocFile.ExpiryDate,
                    DocId = travelDocFile.DocId.ToUpper(),
                    Size = travelDocFile.Size,
                    UploadedDate = DateTime.Now,
                };

                await _unitOfWork.TravelDocumentFileDataRepository.AddAsync(travelDocuments);
                _unitOfWork.Complete();
                return travelDocuments;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding a travel document file : {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task<IEnumerable<TravelDocumentViewModel>> GetDocumentDetailOnEmployeeScreen(int employeeId, HttpContext httpContext)
        {
            try
            {
                IEnumerable<TravelDocumentFileDataModel> travelDocumentsData = await _unitOfWork.TravelDocumentFileDataRepository.GetAllAsync();

                var urlRequest = httpContext.Request;

                var travelDocumentViewModelData = (from travelDocuments in travelDocumentsData
                                                   where travelDocuments.UploadedBy == employeeId
                                                   select new TravelDocumentViewModel
                                                   {
                                                       IdentificationNumber = travelDocuments.DocId,
                                                       UploadedDate = travelDocuments.UploadedDate,
                                                       ValidThru = travelDocuments.ExpiryDate,
                                                       Country = travelDocuments.Country,
                                                       DocumentType = travelDocuments.TravelDocType,
                                                       DocumentSize = travelDocuments.Size,
                                                       DocumentURL = $"{urlRequest.Scheme}://{urlRequest.Host}/{travelDocuments.FilePath}/{travelDocuments.FileName}"
                                                   }).ToList();

                return travelDocumentViewModelData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting the travel documents : {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
