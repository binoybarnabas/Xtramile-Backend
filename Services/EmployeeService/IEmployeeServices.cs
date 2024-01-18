using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.EmployeeService
{
    public interface IEmployeeServices
    {

        public Task<IEnumerable<TBL_EMPLOYEE>> GetEmployeeAsync();

        public Task SetEmployeeAsync(TBL_EMPLOYEE employee);
    }
}
