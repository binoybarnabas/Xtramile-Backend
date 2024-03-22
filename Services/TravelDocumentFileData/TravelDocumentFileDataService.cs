﻿using Microsoft.AspNetCore.Mvc;
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
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
                
                var urlRequest = httpContext.Request;

                var travelDocuments = (from travelDocument in travelDocumentsData
                                       join employee in employeeData on travelDocument.UploadedBy equals employee.EmpId
                                       select new TravelDocumentViewModel
                                       {
                                           UploadedBy = employee.FirstName + " " + employee.LastName,
                                           IdentificationNumber = travelDocument.DocId,
                                           ExpiryDate = travelDocument.ExpiryDate,
                                           Country = travelDocument.Country,
                                           DocumentSize = travelDocument.Size,
                                           DocumentType = travelDocument.TravelDocType,
                                           DocumentURL = $"{urlRequest.Scheme}://{urlRequest.Host}/{travelDocument.FilePath}/{Uri.EscapeDataString(travelDocument.FileName)}",
                                           //+1 is used to include the current Date in the subtraction below
                                           RemainingDays = travelDocument.ExpiryDate.HasValue ? (travelDocument.ExpiryDate.Value - DateTime.Now).Days + 1 : null
                                       }).OrderByDescending(travelDocuments => travelDocuments.ExpiryDate).ToList();

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
        public async Task<IEnumerable<TravelDocumentViewModel>> GetFilteredDocumentsOnTAScreen(string fileType, HttpContext httpContext)
        {
            try
            {
                IEnumerable<TravelDocumentFileDataModel> travelDocumentsData = await _unitOfWork.TravelDocumentFileDataRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var urlRequest = httpContext.Request;

                var travelDocuments = (from travelDocument in travelDocumentsData
                                       join employee in employeeData on travelDocument.UploadedBy equals employee.EmpId
                                       where travelDocument.TravelDocType == fileType
                                       select new TravelDocumentViewModel
                                       {
                                           UploadedBy = employee.FirstName + " " + employee.LastName,
                                           IdentificationNumber = travelDocument.DocId,
                                           ExpiryDate = travelDocument.ExpiryDate,
                                           Country = travelDocument.Country,
                                           DocumentSize = travelDocument.Size,
                                           DocumentType = travelDocument.TravelDocType,
                                           DocumentURL = $"{urlRequest.Scheme}://{urlRequest.Host}/{travelDocument.FilePath}/{Uri.EscapeDataString(travelDocument.FileName)}",
                                           RemainingDays = travelDocument.ExpiryDate.HasValue ? (travelDocument.ExpiryDate.Value - DateTime.Now).Days + 1 : null
                                       }).OrderBy(travelDocuments => travelDocuments.ExpiryDate).ToList();

                return travelDocuments;
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
