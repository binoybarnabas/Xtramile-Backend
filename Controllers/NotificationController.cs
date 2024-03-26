using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.NotificationService;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService) {
            _notificationService = notificationService;
        }
       
        [HttpGet("getnotifications/{employeeId}")]
        public async Task<IActionResult> GetNotifications(int employeeId)
        {
            try {
                IEnumerable<Notifications> notifications = await _notificationService.GetNotifications(employeeId);
                return Ok(notifications);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
          
        }

        
    }
}
