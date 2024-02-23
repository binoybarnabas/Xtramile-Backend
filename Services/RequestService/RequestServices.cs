using Azure.Core;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Dynamic;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;
using XtramileBackend.Utils;

namespace XtramileBackend.Services.RequestService
{
    public class RequestServices : IRequestServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private Random random;

        public RequestServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));


            // Initialize Random with a unique seed (e.g., based on the current time)
            random = new Random(Guid.NewGuid().GetHashCode());
        }


        public Task<IEnumerable<TBL_REQUEST>> GetAllRequestAsync()
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

        public async Task AddRequestAsync(TBL_REQUEST request)
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
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();

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


        public async Task<TBL_REQUEST> GetRequestById(int id)
        {
            try
            {
                TBL_REQUEST travelRequest = await _unitOfWork.RequestRepository.GetByIdAsync(id);
                return travelRequest;
             
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting travel request with id: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
