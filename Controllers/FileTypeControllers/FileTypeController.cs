using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.FileTypeService;

namespace XtramileBackend.Controllers.FileTypeControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileTypeController : ControllerBase
    {

        private readonly IFileTypeServices _fileTypeServices;

        public FileTypeController(IFileTypeServices fileTypeServices)
        {

            _fileTypeServices = fileTypeServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetFileTypesAsync()
        {
            try
            {
                IEnumerable<TBL_FILE_TYPE> filetypes = await _fileTypeServices.GetFileTypesAsync();
                return Ok(filetypes);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting file type: {ex.Message}");
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddFileTypeAsync([FromBody] TBL_FILE_TYPE files)
        {
            try
            {
                await _fileTypeServices.AddFileTypeAsync(files);
                return Ok(files);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a file type: {ex.Message}");
            }


        }

    }
}
