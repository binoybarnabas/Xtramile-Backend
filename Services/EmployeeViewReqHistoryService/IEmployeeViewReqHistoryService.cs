using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Services.EmployeeViewReqHistoryService
{
	public interface IEmployeeViewReqHistoryService
	{
		public Task<IEnumerable<PendingRequetsViewEmployee>> GetPendingRequestsByEmpId(int empId);
	}
}