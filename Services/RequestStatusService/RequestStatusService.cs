using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.RequestStatusService
{
    public class RequestStatusServices : IRequestStatusServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public RequestStatusServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<TBL_REQ_APPROVE>> GetRequestStatusesAsync()
        {
            try
            {
                var requestStatusData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                return requestStatusData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting request statuses: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task AddRequestStatusAsync(TBL_REQ_APPROVE requestStatus)
        {
            try
            {
                await _unitOfWork.RequestStatusRepository.AddAsync(requestStatus);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding a request status: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
