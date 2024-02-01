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
                IEnumerable<TBL_REQ_APPROVE> statusApprovalMap = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> employeeProjectMap = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();

                var results = (from request in requestData
                               join statusApproval in statusApprovalMap on request.RequestId equals statusApproval.RequestId
                               join primarystatus in statusData on statusApproval.PrimaryStatusId equals primarystatus.StatusId
                               join secondarystatus in statusData on statusApproval.SecondaryStatusId equals secondarystatus.StatusId
                               join employee in employeeData on statusApproval.EmpId equals employee.EmpId
                               join employeeProject in employeeProjectMap on employee.EmpId equals employeeProject.EmpId
                               join project in projectData on employeeProject.ProjectId equals project.ProjectId
                               where secondarystatus.StatusCode == "PE" && statusApproval.EmpId == empId
                               select new PendingRequetsViewEmployee 
                               {
                                   requestCode = request.RequestCode,
                                   projectName = project.ProjectName,
                                   reasonOfTravel = request.TripPurpose,
                                   dateOfTravel = request.DepartureDate
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
