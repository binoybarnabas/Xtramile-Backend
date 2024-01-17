using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.EmployeeService
{
    public interface IEmployeeServices
    {

        public IEnumerable<TBL_EMPLOYEE> GetEmployees();
        public void AddEmployee(TBL_EMPLOYEE priority);
    }
}
