using System;
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

        //Update Req Status
/*        public async Task updateRequestStatusAsync(TBL_REQ_APPROVE reqStatusData)
        {
            try
            {
                await _unitOfWork.RequestStatusRepository.AddAsync(reqStatusData);
                _unitOfWork.Complete();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred while adding a request: {ex.Message}");
                throw; // Re-throw the exception to propagate it

            }
        }
*/
 
        

        //Generate Random Code
        public string GenerateRandomCode(int suffix)
        {
            int prefix = random.Next(100, 1000);
            string randomCode = $"{prefix}{suffix}";
            return randomCode;
        }







    }
}
