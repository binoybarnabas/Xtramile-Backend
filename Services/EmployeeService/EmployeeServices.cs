using XtramileBackend.Models.APIModels;
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

        public async Task<EmployeeInfo> GetEmployeeInfo(int id)
        {
            try
            {
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_DEPARTMENT> departmentData = await _unitOfWork.DepartmentRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectEmployeeMap = await _unitOfWork.ProjectMappingRepository.GetAllAsync();

          
                //get the data of the employee
                var employeeDetail = (from projectEmployee in projectEmployeeMap
                                      join project in projectData on projectEmployee.ProjectId equals project.ProjectId
                                      join employee in employeeData.Where(e=>e.EmpId == id) on projectEmployee.EmpId equals employee.EmpId
                                      join department in departmentData on project.DepartmentId equals department.DepartmentId
                                      join reportsToEmployee in employeeData on employee.ReportsTo equals reportsToEmployee.EmpId
                                      select new EmployeeInfo
                                      {
                                          FirstName = employee.FirstName,
                                          LastName = employee.LastName,
                                          Email = employee.Email,
                                          ContactNumber = employee.ContactNumber,
                                          Address = employee.Address,
                                          DepartmentName = department.DepartmentName,
                                          ProjectCode = project.ProjectCode,
                                          ProjectName = project.ProjectName,
                                          ReportsTo = reportsToEmployee.FirstName
                                          
                                      }).FirstOrDefault();

                
                return employeeDetail;

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

        public async Task<TBL_EMPLOYEE> GetEmployeeByIdAsync(int id) {
            try
            {
                TBL_EMPLOYEE employeeData =  await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
                return employeeData;
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
