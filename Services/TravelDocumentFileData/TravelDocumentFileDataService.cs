using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.FileMetaDataService;
using XtramileBackend.Services.FileTypeService;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.TravelDocumentFileData
{
    public class TravelDocumentFileDataService : ITravelDocumentFileDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileTypeServices _fileTypeServices;
        private readonly IFileMetaDataService _fileMetaDataService;

        public TravelDocumentFileDataService(IUnitOfWork unitOfWork, IFileTypeServices fileTypeServices, IFileMetaDataService fileMetaDataService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _fileTypeServices = fileTypeServices;
            _fileMetaDataService = fileMetaDataService;
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
                    using (var stream = File.Create(filePath))
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

        /// <summary>
        /// Retrieves details of travel documents associated with a specific employee.
        /// </summary>
        /// <param name="employeeId">The ID of the employee whose travel documents are to be retrieved.</param>
        /// <param name="httpContext">The HttpContext containing the HTTP request information.</param>
        /// <returns>An enumerable collection of TravelDocumentViewModel objects containing details of travel documents.</returns>
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
                                                       FileId = travelDocuments.TravelDocFileId,
                                                       IdentificationNumber = travelDocuments.DocId,
                                                       UploadedDate = travelDocuments.UploadedDate,
                                                       ExpiryDate = travelDocuments.ExpiryDate,
                                                       Country = travelDocuments.Country,
                                                       DocumentType = travelDocuments.TravelDocType,
                                                       DocumentSize = travelDocuments.Size,
                                                       DocumentURL = $"{urlRequest.Scheme}://{urlRequest.Host}/{travelDocuments.FilePath}/{Uri.EscapeDataString(travelDocuments.FileName)}"
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

        /// <summary>
        /// Retrieves travel documents for display on the admin screen.
        /// </summary>
        /// <param name="httpContext">The HttpContext containing the request details.</param>
        /// <returns>A task representing the asynchronous operation that yields a collection of TravelDocumentViewModel objects.</returns>
        public async Task<IEnumerable<TravelDocumentViewModel>> GetDocumentsOnTravelAdminScreen(HttpContext httpContext)
        {
            try
            {
                IEnumerable<TravelDocumentFileDataModel> travelDocumentsData = await _unitOfWork.TravelDocumentFileDataRepository.GetAllAsync();
                IEnumerable<Employee> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
                
                var urlRequest = httpContext.Request;

                var travelDocuments = (from travelDocument in travelDocumentsData
                                       join employee in employeeData on travelDocument.UploadedBy equals employee.EmpId
                                       select new TravelDocumentViewModel
                                       {
                                           UploadedBy = employee.FirstName + " " + employee.LastName,
                                           IdentificationNumber = travelDocument.DocId,
                                           UploadedDate = travelDocument.UploadedDate,
                                           ExpiryDate = travelDocument.ExpiryDate,
                                           Country = travelDocument.Country,
                                           DocumentSize = travelDocument.Size,
                                           DocumentType = travelDocument.TravelDocType,
                                           DocumentURL = $"{urlRequest.Scheme}://{urlRequest.Host}/{travelDocument.FilePath}/{Uri.EscapeDataString(travelDocument.FileName)}",
                                           RemainingDays = travelDocument.ExpiryDate.HasValue ? (travelDocument.ExpiryDate.Value.Date - DateTime.Today).Days : null
                                       }).OrderBy(travelDocuments => travelDocuments.RemainingDays.HasValue ? (travelDocuments.RemainingDays) : int.MaxValue) //to show docs with null expiry date
                                       .ToList();

                return travelDocuments;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting the travel documents : {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        /// <summary>
        /// Retrieves filtered travel documents based on the specified FileType for display on the Travel Admin screen.
        /// </summary>
        /// <param name="FileType">The type of travel document to filter by.</param>
        /// <param name="httpContext">The HttpContext containing the request information.</param>
        /// <returns>A collection of TravelDocumentViewModel objects representing the filtered travel documents.</returns>
        public async Task<PageinatedResult<TravelDocumentViewModel>> GetFilteredDocumentsOnTAScreen(string fileType, HttpContext httpContext, int pageNumber, int itemsPerPage)
        {
            try
            {
                IEnumerable<TravelDocumentFileDataModel> travelDocumentsData = await _unitOfWork.TravelDocumentFileDataRepository.GetAllAsync();
                IEnumerable<Employee> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var urlRequest = httpContext.Request;

                var travelDocuments = (from travelDocument in travelDocumentsData
                                       join employee in employeeData on travelDocument.UploadedBy equals employee.EmpId
                                       where travelDocument.TravelDocType == fileType
                                       select new TravelDocumentViewModel
                                       {
                                           UploadedBy = employee.FirstName + " " + employee.LastName,
                                           IdentificationNumber = travelDocument.DocId,
                                           UploadedDate = travelDocument.UploadedDate,
                                           ExpiryDate = travelDocument.ExpiryDate,
                                           Country = travelDocument.Country,
                                           DocumentSize = travelDocument.Size,
                                           DocumentType = travelDocument.TravelDocType,
                                           DocumentURL = $"{urlRequest.Scheme}://{urlRequest.Host}/{travelDocument.FilePath}/{Uri.EscapeDataString(travelDocument.FileName)}",
                                           RemainingDays = travelDocument.ExpiryDate.HasValue ? (travelDocument.ExpiryDate.Value.Date - DateTime.Today).Days : null
                                       }).OrderBy(travelDocuments => travelDocuments.RemainingDays.HasValue ? (travelDocuments.RemainingDays) : int.MaxValue); //to show docs with null expiry date

                int totalCount = travelDocuments.Count();

                var pagedTravelDocuments = travelDocuments.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList();

                return new PageinatedResult<TravelDocumentViewModel>
                {
                    Items = pagedTravelDocuments,
                    TotalCount = totalCount,
                };
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting the travel documents : {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        /// <summary>
        /// Retrieves a list of expired travel documents based on the specified file type.
        /// </summary>
        /// <param name="fileType">The type of travel document to filter by.</param>
        /// <param name="httpContext">The HttpContext associated with the current request.</param>
        /// <returns>
        /// A collection of TravelDocumentViewModel objects representing expired travel documents.
        /// </returns>
        public async Task<PageinatedResult<TravelDocumentViewModel>> GetExpiredDocuments(string fileType, HttpContext httpContext, int pageNumber, int itemsPerPage)
        {
            try
            {
                IEnumerable<TravelDocumentFileDataModel> travelDocumentsData = await _unitOfWork.TravelDocumentFileDataRepository.GetAllAsync();
                IEnumerable<Employee> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var urlRequest = httpContext.Request;

                var travelDocuments = (from travelDocument in travelDocumentsData
                                       join employee in employeeData on travelDocument.UploadedBy equals employee.EmpId
                                       where travelDocument.TravelDocType == fileType && travelDocument.ExpiryDate < DateTime.Today
                                       select new TravelDocumentViewModel
                                       {
                                           UploadedBy = employee.FirstName + " " + employee.LastName,
                                           IdentificationNumber = travelDocument.DocId,
                                           UploadedDate = travelDocument.UploadedDate,
                                           ExpiryDate = travelDocument.ExpiryDate,
                                           Country = travelDocument.Country,
                                           DocumentSize = travelDocument.Size,
                                           DocumentType = travelDocument.TravelDocType,
                                           DocumentURL = $"{urlRequest.Scheme}://{urlRequest.Host}/{travelDocument.FilePath}/{Uri.EscapeDataString(travelDocument.FileName)}",
                                           RemainingDays = travelDocument.ExpiryDate.HasValue ? (travelDocument.ExpiryDate.Value.Date - DateTime.Today).Days : null
                                       }).OrderBy(travelDocuments => travelDocuments.RemainingDays.HasValue ? (travelDocuments.RemainingDays) : int.MaxValue); //to show docs with null expiry date

                int totalCount = travelDocuments.Count();

                var pagedTravelDocuments = travelDocuments.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList();

                return new PageinatedResult<TravelDocumentViewModel>
                {
                    Items = pagedTravelDocuments,
                    TotalCount = totalCount,
                };
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting the travel documents : {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        /// <summary>
        /// Deletes the travel document associated with the specified FileId.
        /// </summary>
        /// <param name="FileId">The unique identifier of the travel document to delete.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task DeleteTravelDocument(int FileId)
        {
            try
            {
                TravelDocumentFileDataModel travelDocument = await _unitOfWork.TravelDocumentFileDataRepository.GetByIdAsync(FileId);
                if(travelDocument != null)
                {
                    string filePath = travelDocument.FilePath + "/" + travelDocument.FileName;
                    File.Delete(filePath);
                    _unitOfWork.TravelDocumentFileDataRepository.Delete(travelDocument);
                    await _unitOfWork.SaveChangesAsyn();
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while deleting the travel document : {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        /// <summary>
        /// Retrieves a list of valid travel documents based on the specified file type.
        /// </summary>
        /// <param name="fileType">The type of travel document to filter by.</param>
        /// <param name="httpContext">The HttpContext associated with the current request.</param>
        /// <returns>
        /// A collection of TravelDocumentViewModel objects representing valid travel documents.
        /// </returns>
        public async Task<PageinatedResult<TravelDocumentViewModel>> GetValidDocuments(string fileType, HttpContext httpContext, int pageNumber, int itemsPerPage)
        {
            try
            {
                IEnumerable<TravelDocumentFileDataModel> travelDocumentsData = await _unitOfWork.TravelDocumentFileDataRepository.GetAllAsync();
                IEnumerable<Employee> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var urlRequest = httpContext.Request;

                var travelDocuments = (from travelDocument in travelDocumentsData
                                       join employee in employeeData on travelDocument.UploadedBy equals employee.EmpId
                                       where travelDocument.TravelDocType == fileType && travelDocument.ExpiryDate >= DateTime.Today
                                       select new TravelDocumentViewModel
                                       {
                                           UploadedBy = employee.FirstName + " " + employee.LastName,
                                           IdentificationNumber = travelDocument.DocId,
                                           UploadedDate = travelDocument.UploadedDate,
                                           ExpiryDate = travelDocument.ExpiryDate,
                                           Country = travelDocument.Country,
                                           DocumentSize = travelDocument.Size,
                                           DocumentType = travelDocument.TravelDocType,
                                           DocumentURL = $"{urlRequest.Scheme}://{urlRequest.Host}/{travelDocument.FilePath}/{Uri.EscapeDataString(travelDocument.FileName)}",
                                           RemainingDays = travelDocument.ExpiryDate.HasValue ? (travelDocument.ExpiryDate.Value.Date - DateTime.Today).Days : null
                                       }).OrderBy(travelDocuments => travelDocuments.RemainingDays.HasValue ? (travelDocuments.RemainingDays) : int.MaxValue); //to show docs with null expiry date

                int totalCount = travelDocuments.Count();

                var pagedTravelDocuments = travelDocuments.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList();

                return new PageinatedResult<TravelDocumentViewModel>
                {
                    Items = pagedTravelDocuments,
                    TotalCount = totalCount,
                };
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting the travel documents : {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task<RelevantDocument> GetAllRelevantDocuments(int requestId, HttpContext httpContext)
        {
            try
            {
                Request request = await _unitOfWork.RequestRepository.GetByIdAsync(requestId);
                FileMetaData? travelAuthDocumentData = await _fileMetaDataService.GetFilePathByRequestIdAndDescriptionAsync(requestId, "travelAuthorizationEmailCapture");
                IEnumerable<TravelDocumentFileDataModel> travelDocumentsData = await _unitOfWork.TravelDocumentFileDataRepository.GetAllAsync();
                var urlRequest = httpContext.Request;

                var travelAuthDocument = travelAuthDocumentData != null ? (new TravelDocumentViewModel
                {
                    Filename = travelAuthDocumentData.FileName,
                    DocumentType = travelAuthDocumentData.Description,
                    DocumentSize = "",
                    DocumentURL = $"{urlRequest.Scheme}://{urlRequest.Host}/{travelAuthDocumentData.FilePath}/{Uri.EscapeDataString(travelAuthDocumentData.FileName)}"
                }) : null;

                var passportDocument = (from document in travelDocumentsData 
                                         where document.UploadedBy == request.CreatedBy && document.TravelDocType == "Passport"
                                         && document.ExpiryDate >= DateTime.Today
                                         select new TravelDocumentViewModel
                                         {
                                             Filename = document.FileName,
                                             DocumentType = document.TravelDocType,
                                             DocumentSize = document.Size,
                                             DocumentURL = $"{urlRequest.Scheme}://{urlRequest.Host}/{document.FilePath}/{Uri.EscapeDataString(document.FileName)}"
                                         }).OrderByDescending(passportDocument => passportDocument.ExpiryDate).FirstOrDefault();

                var visaDocument = (from document in travelDocumentsData
                                   where document.UploadedBy == request.CreatedBy && document.TravelDocType == "Visa"
                                   && document.ExpiryDate >= DateTime.Today && document.Country == request.DestinationCountry
                                   select new TravelDocumentViewModel
                                   {
                                       Filename = document.FileName,
                                       DocumentType = document.TravelDocType,
                                       DocumentSize = document.Size,
                                       DocumentURL = $"{urlRequest.Scheme}://{urlRequest.Host}/{document.FilePath}/{Uri.EscapeDataString(document.FileName)}"
                                   }).OrderByDescending(visaDocument => visaDocument.ExpiryDate).FirstOrDefault();

                var idCardDocuments = (from document in travelDocumentsData
                                      where document.UploadedBy == request.CreatedBy && document.TravelDocType == "ID Card"
                                      select new TravelDocumentViewModel
                                      {
                                          Filename = document.FileName,
                                          DocumentType = document.TravelDocType,
                                          DocumentSize = document.Size,
                                          DocumentURL = $"{urlRequest.Scheme}://{urlRequest.Host}/{document.FilePath}/{Uri.EscapeDataString(document.FileName)}"
                                      }).ToList();

                return new RelevantDocument
                {
                    TravelAuthDocument = travelAuthDocument,
                    PassportDocument = passportDocument != null ? passportDocument : null,
                    VisaDocument = visaDocument != null ? visaDocument : null,
                    IdCardDocuments = idCardDocuments
                };
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting the travel documents : {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task<IEnumerable<TravelDocumentViewModel>> GetTravelDocumentByEmployeeName(string fileType, string employeeName, int filterId, HttpContext httpContext)
        {
            try
            {
                IEnumerable<TravelDocumentFileDataModel> travelDocumentsData = await _unitOfWork.TravelDocumentFileDataRepository.GetAllAsync();
                IEnumerable<Employee> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var urlRequest = httpContext.Request;

                var alltravelDocuments = (from travelDocument in travelDocumentsData
                                       join employee in employeeData on travelDocument.UploadedBy equals employee.EmpId
                                       where travelDocument.TravelDocType == fileType &&
                                       (employee.FirstName + " " + employee.LastName).ToLower().Contains(employeeName.ToLower())
                                       select new TravelDocumentViewModel
                                       {
                                           UploadedBy = employee.FirstName + " " + employee.LastName,
                                           IdentificationNumber = travelDocument.DocId,
                                           UploadedDate = travelDocument.UploadedDate,
                                           ExpiryDate = travelDocument.ExpiryDate,
                                           Country = travelDocument.Country,
                                           DocumentSize = travelDocument.Size,
                                           DocumentType = travelDocument.TravelDocType,
                                           DocumentURL = $"{urlRequest.Scheme}://{urlRequest.Host}/{travelDocument.FilePath}/{Uri.EscapeDataString(travelDocument.FileName)}",
                                           RemainingDays = travelDocument.ExpiryDate.HasValue ? (travelDocument.ExpiryDate.Value.Date - DateTime.Today).Days : null
                                       }).OrderBy(travelDocuments => travelDocuments.RemainingDays.HasValue ? (travelDocuments.RemainingDays) : int.MaxValue); //to show docs with null expiry date

                var validtravelDocuments = (from travelDocument in travelDocumentsData
                                       join employee in employeeData on travelDocument.UploadedBy equals employee.EmpId
                                       where travelDocument.TravelDocType == fileType && travelDocument.ExpiryDate >= DateTime.Today &&
                                       (employee.FirstName + " " + employee.LastName).ToLower().Contains(employeeName.ToLower())
                                            select new TravelDocumentViewModel
                                       {
                                           UploadedBy = employee.FirstName + " " + employee.LastName,
                                           IdentificationNumber = travelDocument.DocId,
                                           UploadedDate = travelDocument.UploadedDate,
                                           ExpiryDate = travelDocument.ExpiryDate,
                                           Country = travelDocument.Country,
                                           DocumentSize = travelDocument.Size,
                                           DocumentType = travelDocument.TravelDocType,
                                           DocumentURL = $"{urlRequest.Scheme}://{urlRequest.Host}/{travelDocument.FilePath}/{Uri.EscapeDataString(travelDocument.FileName)}",
                                           RemainingDays = travelDocument.ExpiryDate.HasValue ? (travelDocument.ExpiryDate.Value.Date - DateTime.Today).Days : null
                                       }).OrderBy(travelDocuments => travelDocuments.RemainingDays.HasValue ? (travelDocuments.RemainingDays) : int.MaxValue); //to show docs with null expiry date

                var expiredtravelDocuments = (from travelDocument in travelDocumentsData
                                       join employee in employeeData on travelDocument.UploadedBy equals employee.EmpId
                                       where travelDocument.TravelDocType == fileType && travelDocument.ExpiryDate < DateTime.Today &&
                                       (employee.FirstName + " " + employee.LastName).ToLower().Contains(employeeName.ToLower())
                                              select new TravelDocumentViewModel
                                       {
                                           UploadedBy = employee.FirstName + " " + employee.LastName,
                                           IdentificationNumber = travelDocument.DocId,
                                           UploadedDate = travelDocument.UploadedDate,
                                           ExpiryDate = travelDocument.ExpiryDate,
                                           Country = travelDocument.Country,
                                           DocumentSize = travelDocument.Size,
                                           DocumentType = travelDocument.TravelDocType,
                                           DocumentURL = $"{urlRequest.Scheme}://{urlRequest.Host}/{travelDocument.FilePath}/{Uri.EscapeDataString(travelDocument.FileName)}",
                                           RemainingDays = travelDocument.ExpiryDate.HasValue ? (travelDocument.ExpiryDate.Value.Date - DateTime.Today).Days : null
                                       }).OrderBy(travelDocuments => travelDocuments.RemainingDays.HasValue ? (travelDocuments.RemainingDays) : int.MaxValue); //to show docs with null expiry date

                if (filterId == 0)
                    return expiredtravelDocuments;
                else if (filterId == 1)
                    return validtravelDocuments;
                else
                    return alltravelDocuments;
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
