using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.PriorityService;

namespace XtramileBackend.Controllers.PriorityControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriorityController : ControllerBase
    {

        private readonly IPriorityServices _priorityServices;

        public PriorityController(IPriorityServices priorityServices) {

            _priorityServices = priorityServices;
        }

        [HttpGet]
        public IActionResult GetPriorities() {
             IEnumerable<TBL_PRIORITY> priorityData= _priorityServices.GetPriorities();
            return Ok(priorityData);
       
        }

        [HttpPost]
        public IActionResult AddPriority([FromBody] TBL_PRIORITY Priority)
        {
            _priorityServices.AddPriority(Priority);
            return Ok(Priority);

        }

    }
}
