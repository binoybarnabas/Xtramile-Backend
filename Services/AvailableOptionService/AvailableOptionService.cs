    using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.FileMetaDataService;
using XtramileBackend.Services.FileTypeService;
using XtramileBackend.Services.RequestService;
using XtramileBackend.Services.RequestStatusService;
using XtramileBackend.UnitOfWork;
using AvailableOption = XtramileBackend.Models.EntityModels.AvailableOption;

namespace XtramileBackend.Services.AvailableOptionService
{
    public class AvailableOptionServices : IAvailableOptionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestStatusServices _requestStatusService;
        private readonly IRequestServices _requestServices;
        private readonly IFileTypeServices _fileTypeServices;
        private readonly IFileMetaDataService _fileMetaDataServices;
        public AvailableOptionServices(IRequestServices requestServices, IUnitOfWork unitOfWork, IRequestStatusServices requestStatusServices, IFileTypeServices fileTypeServices, IFileMetaDataService fileMetaDataServices)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _requestStatusService = requestStatusServices;
            _requestServices = requestServices;
            _fileTypeServices = fileTypeServices;
            _fileMetaDataServices = fileMetaDataServices;
        }

        public async Task<IEnumerable<AvailableOption>> GetAvailableOptionsAsync()
        {
            try
            {
                var availableOptionData = await _unitOfWork.AvailableOptionRepository.GetAllAsync();
                return availableOptionData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting available options: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task AddAvailableOptionAsync(AvailableOption availableOption)
        {
            try
            {
                await _unitOfWork.AvailableOptionRepository.AddAsync(availableOption);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding an available option: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        //New Travel Option
        public async Task<int> AddNewTravelOptionAsync(TravelOption travelOption)
        {
            try
            {
                /*await _unitOfWork.TravelOptionRepository.AddAsync(travelOption);
                _unitOfWork.Complete();*/

                await _unitOfWork.TravelOptionRepository.AddAsync(travelOption);
                _unitOfWork.Complete(); // Assuming CompleteAsync returns Task
                return travelOption.OptionId;

            }
            catch(Exception ex) {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding an available option: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
            
        }

        //implement it
        public async Task UpdateFileIdOfOptionAsync(int fileId, int optionId)
        {
            try
            {
                TravelOption travelOption = await _unitOfWork.TravelOptionRepository.GetByIdAsync(optionId);
                if (travelOption != null)
                {
                    // Update the fileId
                    travelOption.FileId = fileId;

                    // Save the changes
                    _unitOfWork.TravelOptionRepository.Update(travelOption);
                    _unitOfWork.Complete();
                }
                else
                {
                    Console.WriteLine($"Option with ID {optionId} not found.");
                }
/*                _unitOfWork.Complete();
*/            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while updating file id: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task<IEnumerable<TravelOption>> GetTravelOptionsByRequestIdAsync(int reqId, bool travelOptiontext)
        {
            try
            {
                var allOptions = await _unitOfWork.TravelOptionRepository.GetAllAsync();
                

                var travelOptions = (travelOptiontext == true) ?
                                     (from item in allOptions
                                      where item.RequestId == reqId && item.FileId == null
                                      select item).ToList():
                                     (from item in allOptions
                                     where item.RequestId == reqId && item.FileId != null
                                      select item).ToList();

                return travelOptions;
            }
            catch (Exception ex )
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting options : {ex.Message}");
                throw; // Re-throw the exception to propagate it

            }
        }

        /// <summary>
        /// To add available travel option ticket details in the form of text
        /// </summary>
        /// <param name="availableOption"></param>
        /// <returns></returns>
        public async Task<string> AddAvailableTextOptionAsync(AvailableOptionText availableOption)
        {
            try{

                TravelOption availableTravelOption = new TravelOption();
                availableTravelOption.RequestId = availableOption.RequestId;
                availableTravelOption.Description = availableOption.HtmlContent;
                availableTravelOption.FileId = null;

                await _unitOfWork.TravelOptionRepository.AddAsync(availableTravelOption);
                _unitOfWork.Complete();
                return "successfully inserted";

            }
            catch (Exception ex) { 
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting adding text available options : {ex.Message}");
                throw; // Re-throw the exception to propagate ;
            }

        }

        public async Task DeleteTravelOptions(int[] FileIds)
        {
            try
            {
                for (int i = 0; i < FileIds.Length; i++)
                {
                    TravelOption travelOptionToBeDeleted = await _unitOfWork.TravelOptionRepository.GetByIdAsync(FileIds[i]);
                    if (travelOptionToBeDeleted != null)
                    {
                        _unitOfWork.TravelOptionRepository.Delete(travelOptionToBeDeleted);
                        await _unitOfWork.SaveChangesAsyn();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while deleting travel options : {ex.Message}");
                throw; // Re-throw the exception to propagate ;
            }

        }


        /// <summary>
        /// addding travel option based on text or images simultaneously
        /// </summary>
        /// <param name="travelOption"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task AddTravelAvailableOption(TravelOptionAPI travelOption, HttpContext httpContext) {
            try
            {
                //Handling text data of travel request
                List<int> optionIds = new List<int>();
                if(travelOption.Description != null)
                {
                    foreach (string description in travelOption.Description)
                    {
                        var tblTravelOption = new TravelOption
                        {
                            RequestId = travelOption.RequestId,
                            Description = description,
                        };
                        int optionId = await AddNewTravelOptionAsync(tblTravelOption);
                        optionIds.Add(optionId);
                    }
                }

                // option descrption index corresponding to a file
                int optionIndex = -1;

                // Check if files are attached and handle them
                if (httpContext.Request.Form.Files != null && httpContext.Request.Form.Files.Count > 0)
                {
                    foreach (var file in httpContext.Request.Form.Files)
                    {
                        string randomCode = _requestServices.GenerateRandomCode(travelOption.RequestId);

                        // Renaming the file using REQCODE
                        var fileName = $"{randomCode}{file.FileName}";

                        // Define the target folder
                        var targetFolder = "Uploads/RequestFiles/TravelOptions";
                        if (!Directory.Exists(targetFolder))
                        {
                            // Create directory
                            try
                            {
                                Directory.CreateDirectory(targetFolder);
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


                        /*                        var filePath = Path.Combine(targetFolder, fileName);
                        */
                        var filePath = Path.Combine(targetFolder, fileName).Replace("\\", "/");

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Get Extension of received file
                        string fileExtension = Path.GetExtension(filePath);

                        // Get file type id based on the file extension of received file
                        int fileTypeId = await _fileTypeServices.GetFileTypeIdByExtensionAsync(fileExtension.Substring(1));

                        // To save the file meta data in TBL_FILE_METADATA
                        var fileMetaData = new FileMetaData
                        {
                            RequestId = travelOption.RequestId,
                            FileName = fileName,
                            FilePath = targetFolder,
                            Description = file.Name, // Assuming file.Name is appropriate for description
                            FileTypeId = fileTypeId,
                            CreatedOn = DateTime.Now,
                            CreatedBy = travelOption.EmpId,
                        };

                        // Adding files meta data
                        await _fileMetaDataServices.AddFileMetaDataAsync(fileMetaData);

                        int fileId = await _fileMetaDataServices.GetFileIdByFileNameAsync(fileName);

                        await UpdateFileIdOfOptionAsync(fileId, optionIds[++optionIndex]);

                    }

                }
                if (travelOption.Texts != null && travelOption.Texts.Length > 0)
                {
                    foreach (string textOptions in travelOption.Texts)
                    {
                        AvailableOptionText option = new AvailableOptionText
                        {
                            HtmlContent = textOptions,
                            RequestId = travelOption.RequestId,
                        };
                        string response = await AddAvailableTextOptionAsync(option);
                        Console.WriteLine(response, "text options");
                    }
                }
                RequestApprove reqStatus = new RequestApprove
                {
                    RequestId = travelOption.RequestId,
                    EmpId = travelOption.EmpId,
                    PrimaryStatusId = travelOption.PrimaryStatusId,
                    SecondaryStatusId = travelOption.SecondaryStatusId,
                    date = DateTime.Now,
                };
                await _requestStatusService.AddRequestStatusAsync(reqStatus);
            }
            catch (Exception ex)
            {
                // Log and handle any exceptions
                Console.WriteLine($"Error processing request: {ex.Message}");
            }

        }

        public async Task UpdateTravelOptionSelected(TravelOptionMap travelOption)
        {
            try
            {
                IEnumerable<TravelOptionMap> travelOptionsData = await _unitOfWork.TravelOptionMappingRepository.GetAllAsync();
                TravelOptionMap? existingTravelOption = travelOptionsData.FirstOrDefault(eto => eto.RequestId == travelOption.RequestId);

                if (existingTravelOption != null)
                {
                    existingTravelOption.EmpId = travelOption.EmpId;
                    existingTravelOption.OptionId = travelOption.OptionId;

                    await _unitOfWork.SaveChangesAsyn();
                }
            }
            catch (Exception ex)
            {
                // Log and handle any exceptions
                Console.WriteLine($"Error Updating Option Mapping: {ex.Message}");
            }
        }

    }
}
