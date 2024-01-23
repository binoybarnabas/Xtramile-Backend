namespace XtramileBackend.Services.EmployeeViewPenReqService
{
    public interface IEmployeeViewPenReqService 
    {
        public Task<IEnumerable<object>> GetPendingRequestsByEmpId(int empId);
    }
}
