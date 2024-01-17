using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.EmployeeService
{
    public class EmployeeServices : IEmployeeServices
    {
        public readonly IUnitOfWork _unitOfWork;
        public EmployeeServices(IUnitOfWork unitOfWork) { 
        _unitOfWork = unitOfWork;
        }

        public IEnumerable<TBL_EMPLOYEE> GetEmployees() { 
            IEnumerable<TBL_EMPLOYEE> EmployeeData = _unitOfWork.EmployeeRepository.GetAll();
            return EmployeeData;

        }

        public void AddEmployee(TBL_EMPLOYEE Employee) {

            _unitOfWork.EmployeeRepository.Add(Employee);
            _unitOfWork.Complete();
        }
    }
}
