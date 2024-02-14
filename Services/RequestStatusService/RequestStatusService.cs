using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XtramileBackend.Models.APIModels;
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
                requestStatus.date = DateTime.Now;
                await _unitOfWork.RequestStatusRepository.AddAsync(requestStatus);
                _unitOfWork.Complete();

                if(requestStatus.PrimaryStatusId == 10 && requestStatus.SecondaryStatusId == 10)
                {

                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding a request status: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
        public async Task<string> GetRequestStatusNameAsync(int requestId)
        {
            try
            {
                IEnumerable<TBL_REQ_APPROVE> statusApprovalMap = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();

                var result = (from statusApproval in statusApprovalMap
                              join status in statusData on statusApproval.PrimaryStatusId equals status.StatusId
                              where statusApproval.RequestId == requestId
                              select new PendingRequetsViewEmployee
                              {
                                  statusName = status.StatusName
                              }).LastOrDefault();
                return result?.statusName ?? "undefined";
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting pending requests: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
