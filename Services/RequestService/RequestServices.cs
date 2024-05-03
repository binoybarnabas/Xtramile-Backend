using Azure.Core;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Dynamic;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.FileMetaDataService;
using XtramileBackend.UnitOfWork;
using XtramileBackend.Utils;
using Request = XtramileBackend.Models.EntityModels.Request;

namespace XtramileBackend.Services.RequestService
{
    public class RequestServices : IRequestServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private Random random;
        private readonly IFileMetaDataService _fileMetaDataService;

        public RequestServices(IUnitOfWork unitOfWork, IFileMetaDataService fileMetaDataService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _fileMetaDataService = fileMetaDataService;
            // Initialize Random with a unique seed (e.g., based on the current time)
            random = new Random(Guid.NewGuid().GetHashCode());
        }


        public Task<IEnumerable<Request>> GetAllRequestAsync()
        {
            try
            {
                var request = _unitOfWork.RequestRepository.GetAllAsync();
                return request;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting request: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }

        }

        public async Task AddRequestAsync(Request request)
        {
            try
            {
                await _unitOfWork.RequestRepository.AddAsync(request);         
                 _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding a request: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }

        }


        //Generate Random Code
        public string GenerateRandomCode(int suffix)
        {
            int prefix = random.Next(100, 1000);
            string randomCode = $"{prefix}{suffix}";
            return randomCode;
        }


        // Async Method to get request ID by accepting EmpID as an argument
        public async Task<int> GetRequestIdByRequestCode(string requestCode)
        {
            try
            {
                IEnumerable<Request> requestData = await _unitOfWork.RequestRepository.GetAllAsync();

                var requestId = (from item in requestData
                                 where item.RequestCode == requestCode
                                 select item.RequestId).FirstOrDefault();

                return requestId;

            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting request id: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
            //EOF
        }


        public async Task<Request> GetRequestById(int id)
        {
            try
            {
                Request travelRequest = await _unitOfWork.RequestRepository.GetByIdAsync(id);
                return travelRequest;

            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting travel request with id: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        /// <summary>
        /// Function to get the Reason Description for a particular request
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public async Task<string> GetReasonDescriptionByRequestId(int requestId)
        {
            try
            {
                Request request = await _unitOfWork.RequestRepository.GetByIdAsync(requestId);
                IEnumerable<Reason> reasonData = await _unitOfWork.ReasonRepository.GetAllAsync();

                string? reasonDescription = (from reason in reasonData
                                            where reason.ReasonId == request.ReasonId
                                            select reason.Description).FirstOrDefault();

                return reasonDescription;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting reason description: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task UpdateRequestDetails(TravelRequestViewModel requestData, HttpContext httpContext)
        {
            try
            {
                Request existingRequest = await _unitOfWork.RequestRepository.GetByIdAsync(requestData.RequestId);
                int fileId = await _fileMetaDataService.GetFileIdByRequestIdAndTravelAuthFile(requestData.RequestId);
                FileMetaData existingfileData = await _unitOfWork.FileMetaDataRepository.GetByIdAsync(fileId);
                string fileName = "";
                string targetFolder = "Uploads/RequestFiles/TravelAuthorizationEmails";


                if (httpContext.Request.Form.Files != null)
                {
                    var file = httpContext.Request.Form.Files[0];
                    fileName = $"{requestData.RequestCode}{file.FileName}";
                    var filePath = Path.Combine(targetFolder, fileName).Replace("\\", "/");
                    using (var stream = File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                if (existingRequest != null)
                {
                    existingRequest.RequestId = requestData.RequestId;
                    existingRequest.DepartureDate = DateTime.Parse(requestData.DepartureDate);
                    existingRequest.ReturnDate = requestData.ReturnDate != null ? DateTime.Parse(requestData.ReturnDate) : null;
                    existingRequest.TravelType = requestData.TravelType;
                    existingRequest.PerdiemId = null;
                    existingRequest.CreatedBy = existingRequest.CreatedBy;
                    existingRequest.CreatedOn = existingRequest.CreatedOn;
                    existingRequest.ModifiedBy = int.Parse(requestData.CreatedBy);
                    existingRequest.ModifiedOn = DateTime.Now;
                    existingRequest.SourceCity = requestData.SourceCity;
                    existingRequest.SourceState = null;
                    existingRequest.SourceCountry = requestData.SourceCountry;
                    existingRequest.DestinationCity = requestData.DestinationCity;
                    existingRequest.DestinationState = null;
                    existingRequest.DestinationCountry = requestData.DestinationCountry;
                    existingRequest.CabRequired = requestData.CabRequired;
                    existingRequest.AccommodationRequired = requestData.AccommodationRequired;
                    existingRequest.SourceCityZipCode = null;
                    existingRequest.DestinationCityZipCode = null;
                    existingRequest.TripPurpose = requestData.TripPurpose;
                    existingRequest.PrefDepartureTime = requestData.PrefDepartureTime;
                    existingRequest.PriorityId = null;
                    existingRequest.RequestCode = requestData.RequestCode;
                    existingRequest.AdditionalComments = null;
                    existingRequest.ReasonId = null;
                    existingRequest.ProjectId = int.Parse(requestData.ProjectId);
                    existingRequest.TripType = requestData.TripType;
                    existingRequest.TravelModeId = int.Parse(requestData.TravelModeId);
                    existingRequest.PrefPickUpTime = requestData.PrefPickUpTime;

                    if(existingfileData != null)
                    {
                        var filePath = Path.Combine(targetFolder, existingfileData.FileName).Replace("\\", "/");
                        File.Delete(filePath);
                        existingfileData.FileName = fileName;
                        existingfileData.ModifiedBy = int.Parse(requestData.CreatedBy);
                        existingfileData.ModifiedOn = DateTime.Now;
                    }

                    await _unitOfWork.SaveChangesAsyn();
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while updating request details: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
