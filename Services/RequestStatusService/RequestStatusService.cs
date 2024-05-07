using Azure.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.StatusService;
using XtramileBackend.UnitOfWork;
using XtramileBackend.Utils;

namespace XtramileBackend.Services.RequestStatusService
{
    public class RequestStatusServices : IRequestStatusServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly IStatusServices _statusServices;

        public RequestStatusServices(IUnitOfWork unitOfWork, IMailService mailService, IStatusServices statusServices)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
            _statusServices = statusServices;
        }

        public async Task<IEnumerable<RequestApprove>> GetRequestStatusesAsync()
        {
            try
            {
                var requestStatusData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                await _mailService.SendToManagersOnSubmit(1069);
                return requestStatusData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting request statuses: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task AddRequestStatusAsync(RequestApprove requestStatus)
        {
            try
            {
                requestStatus.date = DateTime.Now;
                await _unitOfWork.RequestStatusRepository.AddAsync(requestStatus);
                _unitOfWork.Complete();

                if (requestStatus.PrimaryStatusId == 1 && requestStatus.SecondaryStatusId == 2)
                {
                    //mail to be sent to employee on reuqest submit
                    await _mailService.SendToEmployeeOnSubmit(requestStatus.RequestId);

                    //mail to be sent to reporting manager on request submit
                    await _mailService.SendToManagersOnSubmit(requestStatus.RequestId);

                    //mail to be sent to the travelAdminTeam on request submit
                    await _mailService.SendToTravelAdminTeamOnSubmit(requestStatus.RequestId);
                }

                if (requestStatus.PrimaryStatusId == 12 && requestStatus.SecondaryStatusId == 2)
                {
                    //send mail to employee on manager approval
                    await _mailService.SendToEmployeeOnManagerApproval(requestStatus.RequestId);

                    //send mail to travel admin team on manager approval
                    await _mailService.SendToTravelAdminTeamOnManagerApproval(requestStatus.RequestId);
                }

                if (requestStatus.PrimaryStatusId == 6 && requestStatus.SecondaryStatusId == 2)
                {
                    //mail to be sent to Employee on request denial by manager
                    await _mailService.SendToEmployeeOnManagerDenial(requestStatus.RequestId);

                    //mail to be sent to travel admin team on request denial by a manager
                    await _mailService.SendToTravelAdminTeamOnManagerDenial(requestStatus.RequestId);
                }

                if (requestStatus.PrimaryStatusId == 2 && requestStatus.SecondaryStatusId == 10)
                {
                    //mail to be sent to reporting manager on option sent
                    await _mailService.SendToReportingManagerOnOptionSent(requestStatus.RequestId);
                }

                if (requestStatus.PrimaryStatusId == 2 && requestStatus.SecondaryStatusId == 11)
                {
                    //mail to be sent to travel admin once the manager has picked the travel option
                    await _mailService.SendToTrvaelAdminTeamOnOptionSelection(requestStatus.RequestId);
                }

                if (requestStatus.PrimaryStatusId == 12 && requestStatus.SecondaryStatusId == 12)
                {
                    //mail to be sent to Employee on Travel Admin Approval
                    await _mailService.SendToEmployeeOnTravelAdminApproval(requestStatus.RequestId);
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
                IEnumerable<RequestApprove> statusApprovalMap = await _unitOfWork.RequestStatusRepository.GetAllAsync();

                RequestApprove? requestStatus = statusApprovalMap.LastOrDefault(rs => rs.RequestId == requestId);

                string statusName = requestStatus != null ? _statusServices.GetStatusName(requestStatus.PrimaryStatusId, requestStatus.SecondaryStatusId) : "";

                return statusName;
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
