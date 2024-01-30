using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.EmployeeService
{
    public interface IEmployeeServices
    {

        public Task<IEnumerable<TBL_EMPLOYEE>> GetEmployeeAsync();

        public Task SetEmployeeAsync(TBL_EMPLOYEE employee);

        public Task<TBL_EMPLOYEE> GetEmployeeByIdAsync(int id);

        public Task<EmployeeInfo> GetEmployeeInfo(int id);

        public Task<IEnumerable<OptionCard>> GetOptionsByReqId(int reqId);

    }
}
