using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Services.AuthService;

namespace XtramileBackend.Controllers.AuthController
{
 
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IAuthenticationService authenticationService) {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] Credentials credential) {
            UserDataModel data = _authenticationService.checkCredentials(credential);
            return Ok(data);
        }

        [HttpGet("logininfo")]
        public IActionResult getLoginInfo()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var nameIdentifierClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            var nameClaim = claimsIdentity?.FindFirst(ClaimTypes.Name);
            var roleClaim = claimsIdentity?.FindFirst(ClaimTypes.Role);
            var claimObject = new { Empid = nameIdentifierClaim?.Value, Employeename = nameClaim?.Value, RoleName = roleClaim?.Value };
            return Ok(claimObject);
        }

    }
}
