using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.CountryService;
using XtramileBackend.Services.PerdiemService;

namespace XtramileBackend.Controllers.PerdiemController
{
    [EnableCors("AllowAngularDev")]
    [Route("api/perdiem")]
    [ApiController]
    public class PerdiemController: ControllerBase
    {
        private readonly IPerdiemServices _perdiemServices;

        public PerdiemController(IPerdiemServices perdiemServices)
        {

            _perdiemServices = perdiemServices;
        }

        [HttpGet("perdiems")]
        public async Task<IActionResult> GetPerdiemAsync()
        {
            try
            {
                IEnumerable<TBL_PER_DIEM> perdiemData = await _perdiemServices.GetPerdiemAsync();
                return Ok(perdiemData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving perdiem data : " + ex.Message);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPerdiemAsync([FromBody] TBL_PER_DIEM perdiem)
        {
            try
            {
                await _perdiemServices.AddPerdiemAsync(perdiem);
                return Ok(perdiem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding perdiem : " + ex.Message);
            }
        }

    }
}
