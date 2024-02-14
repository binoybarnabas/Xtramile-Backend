using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;
using XtramileBackend.Utils;

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

                TBL_REQUEST request = await _unitOfWork.RequestRepository.GetByIdAsync(requestStatus.RequestId);
                TBL_EMPLOYEE employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.CreatedBy);
                TBL_EMPLOYEE manager = await _unitOfWork.EmployeeRepository.GetByIdAsync(requestStatus.EmpId);

                //Send an email to employee once an option is sent by the travel admin 
                if (requestStatus.PrimaryStatusId == 10 && requestStatus.SecondaryStatusId == 10)
                {
                    Mail sentOptions = new Mail();

                    if (employee != null)
                    {
                        sentOptions.recipientName = employee.FirstName + " " + employee.LastName;
                        sentOptions.recipientEmail = employee.Email;
                        sentOptions.managerName = manager.FirstName + " " + manager.LastName;
                        sentOptions.requestCode = request.RequestCode;
                        sentOptions.mailContext = "options";
                        MailService.SendMail(sentOptions);
                    }
                }

                //Send an email to travel admins once employee picks an option
                if (requestStatus.PrimaryStatusId == 11 && requestStatus.SecondaryStatusId == 11)
                {
                    List<Mail> travelAdminMails = new List<Mail>();

                    // Retrieve data from repositories
                    IEnumerable<TBL_EMPLOYEE> travelAdminData = await _unitOfWork.EmployeeRepository.GetAllAsync();
                    IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                    IEnumerable<TBL_ROLES> rolesData = await _unitOfWork.RoleRepository.GetAllAsync();
                    IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                    IEnumerable<TBL_DEPARTMENT> departmentData = await _unitOfWork.DepartmentRepository.GetAllAsync();

                    // Query to get travel admin details
                    var travelAdminEmails = (from travelAdmin in travelAdminData
                                             join projectMapping in projectMappingData on travelAdmin.EmpId equals projectMapping.EmpId
                                             join project in projectData on projectMapping.ProjectId equals project.ProjectId
                                             join department in departmentData on project.DepartmentId equals department.DepartmentId
                                             join role in rolesData on travelAdmin.RoleId equals role.RoleId
                                             where department.DepartmentCode == "TA" && (travelAdmin.RoleId == 2 || travelAdmin.RoleId == 3)
                                             select new { travelAdmin.FirstName, travelAdmin.LastName, travelAdmin.Email }).ToList();

                    // Iterate through each travel admin and create a separate mail for each
                    foreach (var travelAdminAndManager in travelAdminEmails)
                    {
                        // Create a new mail instance for each travel admin
                        Mail selectedOption = new Mail();

                        // Set recipient details
                        selectedOption.recipientName = travelAdminAndManager.FirstName + " " + travelAdminAndManager.LastName;
                        selectedOption.recipientEmail = travelAdminAndManager.Email;

                        // Set other mail details
                        selectedOption.requestCode = request.RequestCode;
                        selectedOption.mailContext = "sendToTAOnOptionSelect";
                        selectedOption.requestSubmittedBy = employee.FirstName + " " + employee.LastName;

                        // Add the mail to the list
                        travelAdminMails.Add(selectedOption);
                    }

                    // Send mails to all travel admins
                    foreach (var mail in travelAdminMails)
                    {
                        MailService.SendMail(mail);
                    }
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
