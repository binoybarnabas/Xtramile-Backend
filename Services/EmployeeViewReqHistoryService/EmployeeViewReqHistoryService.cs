using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.EmployeeViewReqHistoryService
{
	public class EmployeeViewReqHistoryService
	{
		private IUnitOfWork _unitOfWork;
		public EmployeeViewReqHistoryService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); ;
		}
		public async Task<IEnumerable<EmployeeViewReq>> GetPendingRequestsByEmpId(int empId)
		{
			try
			{
				IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
				IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
				IEnumerable<TBL_REQ_APPROVE> statusApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
				IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
				IEnumerable<TBL_TRAVEL_TYPE> travelTypeData = await _unitOfWork.TravelTypeRepository.GetAllAsync();
			

				var result = (from request in requestData
							  
							  join statusApproval in statusApprovalData on request.RequestId equals statusApproval.RequestId
							  join project in projectData on statusApproval.ProjectId equals project.ProjectId
							  join primarystatus in statusData on statusApproval.PrimaryStatusId equals primarystatus.StatusId
							  join secondarystatus in statusData on statusApproval.SecondaryStatusId equals secondarystatus.StatusId
							  join travelType in travelTypeData on request.TravelTypeId equals travelType.TravelTypeID

							  where secondarystatus.StatusCode == "CL" || primarystatus.StatusCode=="CL" && request.CreatedBy == empId
							  select new EmployeeViewReq
							  {
								  RequestId = request.RequestId,
								  ProjectCode = project.ProjectCode,
								  ProjectName = project.ProjectName,
								  TravelType = travelType.TypeName,
								  ClosedDate= new DateOnly(statusApproval.date.Year, statusApproval.date.Month, statusApproval.date.Day),
								  Status ="Closed"
								  
							  }).ToList();
				return result;
			}
			catch (Exception ex)
			{
				// Handle or log the exception
				Console.WriteLine($"An error occurred while getting closed requests: {ex.Message}");
				throw; // Re-throw the exception to propagate it
			}
		}


	}
}