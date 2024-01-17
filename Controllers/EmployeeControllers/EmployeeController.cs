using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.EmployeeService;

namespace XtramileBackend.Controllers.EmployeeControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeServices _employeeServices;

        public EmployeeController(IEmployeeServices employeeServices) {
            _employeeServices = employeeServices;
        }


        [HttpGet]
        public IActionResult GetPriorities()
        {
            IEnumerable<TBL_EMPLOYEE> employeeData = _employeeServices.GetEmployees();
            return Ok(employeeData);

        }

        [HttpPost]
        public IActionResult AddPriority([FromBody] TBL_EMPLOYEE Employee)
        {
            _employeeServices.AddEmployee(Employee);
            return Ok(Employee);

        }
    }
}
