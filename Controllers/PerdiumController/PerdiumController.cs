using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.CountryService;
using XtramileBackend.Services.PerdiumService;

namespace XtramileBackend.Controllers.PerdiumController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerdiumController: ControllerBase
    {
        private readonly IPerdiumServices _perdiumServices;

        public PerdiumController(IPerdiumServices perdiumServices)
        {

            _perdiumServices = perdiumServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetPerdiumAsync()
        {
            try
            {
                IEnumerable<TBL_PER_DIUM> perdiumData = await _perdiumServices.GetPerdiumAsync();
                return Ok(perdiumData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving perdium data.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPerdiumAsync([FromBody] TBL_PER_DIUM perdium)
        {
            try
            {
                await _perdiumServices.AddPerdiumAsync(perdium);
                return Ok(perdium);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding perdium.");
            }
        }

    }
}
