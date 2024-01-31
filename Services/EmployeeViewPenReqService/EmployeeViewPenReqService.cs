using Microsoft.EntityFrameworkCore;
using XtramileBackend.Data;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.EmployeeViewPenReqService
{
    public class EmployeeViewPenReqService : IEmployeeViewPenReqService
    {
        private IUnitOfWork _unitOfWork;
        public EmployeeViewPenReqService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); ;
        }
        public async Task<IEnumerable<PendingRequetsViewEmployee>> GetPendingRequestsByEmpId(int empId)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalMap = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_REASON> reasonData = await _unitOfWork.ReasonRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var results = (from request in requestData
                               join project in projectData on request.ProjectId equals project.ProjectId
                               join statusApproval in statusApprovalMap on request.RequestId equals statusApproval.RequestId
                               join primarystatus in statusData on statusApproval.PrimaryStatusId equals primarystatus.StatusId
                               join secondarystatus in statusData on statusApproval.SecondaryStatusId equals secondarystatus.StatusId
                               join reason in reasonData on request.ReasonId equals reason.ReasonId
                               join employee in employeeData on statusApproval.EmpId equals employee.EmpId
                               where secondarystatus.StatusCode == "PE" && request.CreatedBy == empId
                               group new { request, project, statusApproval, primarystatus, secondarystatus, reason, employee } by request.RequestId into requestGroup
                               select requestGroup.Last())
               .Select(x => new PendingRequetsViewEmployee
               {
                   modifiedBy = x.employee.FirstName + x.employee.LastName,
                   statusName = x.primarystatus.StatusName,
                   requestCode = x.request.RequestCode,
                   projectName = x.project.ProjectName,
                   reasonOfTravel = x.reason.Description,
                   dateOfTravel = x.request.DepartureDate
               }).ToList();
                return results;
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
