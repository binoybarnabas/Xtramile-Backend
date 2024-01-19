using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.EmployeeService
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TBL_EMPLOYEE>> GetEmployeeAsync()
        {
            try
            {
                var employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
                return employeeData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting Employees: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task SetEmployeeAsync(TBL_EMPLOYEE employee)
        {
            try
            {
                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while setting Employee: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
