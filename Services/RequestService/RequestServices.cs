using Microsoft.AspNetCore.Http.HttpResults;
using System;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

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


        //New Travel Request
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
        public async Task<int> GetRequestIdByEmpId(int empId)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();

                /*        var requestId = (from item in requestData
                                         where item.CreatedBy == empId
                                         select item.RequestId).FirstOrDefault();
                        return requestId;*/
                

                var requestId = requestData
                .Where(item => item.CreatedBy == empId)
                .Select(item => item.RequestId)
         .          FirstOrDefault();

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











    }
}
