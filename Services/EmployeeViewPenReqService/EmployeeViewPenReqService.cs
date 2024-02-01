/*using Microsoft.EntityFrameworkCore;
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
        }        public async Task<IEnumerable<PendingRequetsViewEmployee>> GetPendingRequestsByEmpId(int empId)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalMap = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_REASON> reasonData = await _unitOfWork.ReasonRepository.GetAllAsync();

                var result = (from request in requestData
                                    join project in projectData on request.ProjectId equals project.ProjectId
                                    join statusApproval in statusApprovalMap on request.RequestId equals statusApproval.RequestId
                                    join primarystatus in statusData on statusApproval.PrimaryStatusId equals primarystatus.StatusId
                                    join secondarystatus in statusData on statusApproval.SecondaryStatusId equals secondarystatus.StatusId
                                    join reason in reasonData on request.ReasonId equals reason.ReasonId
                                    where secondarystatus.StatusCode == "PE" && request.CreatedBy == empId
                                    select new PendingRequetsViewEmployee
                                    {
                                        statusName = primarystatus.StatusName,
                                        requestCode = request.RequestCode,
                                        projectName = project.ProjectName,
                                        reasonOfTravel = reason.Description,
                                        dateOfTravel = request.DepartureDate
                                    }).ToList();
                return result;
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
*/