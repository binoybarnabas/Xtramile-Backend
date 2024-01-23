using Microsoft.EntityFrameworkCore;
using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.EmployeeViewPenReqService
{
    public class EmployeeViewPenReqService : IEmployeeViewPenReqService
    {
        private readonly AppDBContext _context;
        public EmployeeViewPenReqService(AppDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<object>> GetPendingRequestsByEmpId(int empId)
        {
            try
            {
                var result = await (from request in _context.TBL_REQUEST
                                    join project in _context.TBL_PROJECT on request.ProjectId equals project.ProjectId
                                    join statusApproval in _context.TBL_REQ_APPROVE on request.RequestId equals statusApproval.RequestId
                                    join status in _context.TBL_STATUS on statusApproval.SecondaryStatusId equals status.StatusId
                                    join reason in _context.TBL_REASON on request.ReasonId equals reason.ReasonId
                                    where status.StatusCode == "PE" && request.CreatedBy == empId
                                    select new
                                    {
                                        RequestCode = request.RequestCode,
                                        ProjectCode = project.ProjectCode,
                                        ProjectName = project.ProjectName,
                                        ReasonOfTravel = reason.Description,
                                        DateOfTravel = statusApproval.date
                                    }).ToListAsync();
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
