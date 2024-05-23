using Azure.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.StatusService;
using XtramileBackend.UnitOfWork;
using XtramileBackend.Utils;
using Request = XtramileBackend.Models.EntityModels.Request;

namespace XtramileBackend.Services.RequestStatusService
{
    public class RequestStatusServices : IRequestStatusServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStatusServices _statusServices;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RequestStatusServices(IUnitOfWork unitOfWork, IStatusServices statusServices, IServiceScopeFactory serviceScopeFactory)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _statusServices = statusServices;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<IEnumerable<RequestApprove>> GetRequestStatusesAsync()
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

        public async Task AddRequestStatusAsync(RequestApprove requestStatus)
        {
            try
            {
                requestStatus.date = DateTime.Now;
                //Remove all previous records with that requestId and the add the incoming requestStatus mapping to the table
                if (requestStatus.PrimaryStatusId == 3 && requestStatus.SecondaryStatusId == 3)
                {
                    IEnumerable<RequestApprove> requestStatuses = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                    IEnumerable<RequestApprove> recordsToDelet = requestStatuses.Where(rs => rs.RequestId == requestStatus.RequestId).ToList();
                    foreach (RequestApprove record in  recordsToDelet)
                    {
                        _unitOfWork.RequestStatusRepository.Delete(record);
                        await _unitOfWork.SaveChangesAsyn();
                    }  
                    
                    await _unitOfWork.RequestStatusRepository.AddAsync(requestStatus);
                    _unitOfWork.Complete();
                }
                else
                {
                    await _unitOfWork.RequestStatusRepository.AddAsync(requestStatus);
                    _unitOfWork.Complete();
                }


                /*//To run email service as a separate Taks
                _ = Task.Run(async () =>
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var mailService = scope.ServiceProvider.GetService<IMailService>();
                        if (mailService != null)
                        {
                            if (requestStatus.PrimaryStatusId == 1 && requestStatus.SecondaryStatusId == 2)
                            {
                                //mail to be sent to employee on reuqest submit
                                await mailService.SendToEmployeeOnSubmit(requestStatus.RequestId);

                                //mail to be sent to reporting manager on request submit
                                await mailService.SendToManagersOnSubmit(requestStatus.RequestId);

                                //mail to be sent to the travelAdminTeam on request submit
                                await mailService.SendToTravelAdminTeamOnSubmit(requestStatus.RequestId);
                            }

                            if (requestStatus.PrimaryStatusId == 12 && requestStatus.SecondaryStatusId == 2)
                            {
                                //send mail to employee on manager approval
                                await mailService.SendToEmployeeOnManagerApproval(requestStatus.RequestId);

                                //send mail to travel admin team on manager approval
                                await mailService.SendToTravelAdminTeamOnManagerApproval(requestStatus.RequestId);
                            }

                            if (requestStatus.PrimaryStatusId == 6 && requestStatus.SecondaryStatusId == 2)
                            {
                                //mail to be sent to Employee on request denial by manager
                                await mailService.SendToEmployeeOnManagerDenial(requestStatus.RequestId);

                                //mail to be sent to travel admin team on request denial by a manager
                                await mailService.SendToTravelAdminTeamOnManagerDenial(requestStatus.RequestId);
                            }

                            if (requestStatus.PrimaryStatusId == 2 && requestStatus.SecondaryStatusId == 10)
                            {
                                //mail to be sent to reporting manager on option sent
                                await mailService.SendToReportingManagerOnOptionSent(requestStatus.RequestId);
                            }

                            if (requestStatus.PrimaryStatusId == 2 && requestStatus.SecondaryStatusId == 11)
                            {
                                //mail to be sent to travel admin once the manager has picked the travel option
                                await mailService.SendToTrvaelAdminTeamOnOptionSelection(requestStatus.RequestId);
                            }

                            if (requestStatus.PrimaryStatusId == 12 && requestStatus.SecondaryStatusId == 12)
                            {
                                //mail to be sent to Employee on Travel Admin Approval
                                await mailService.SendToEmployeeOnTravelAdminApproval(requestStatus.RequestId);
                            }

                            if(requestStatus.PrimaryStatusId == 4 && requestStatus.SecondaryStatusId == 7)
                            {
                                //mail to be sent to Employee with Travel Ticket
                                await mailService.SendToEmployeeTravelTicket(requestStatus.RequestId);
                            }
                        }
                    }
                });*/
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
