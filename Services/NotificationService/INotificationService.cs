using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.NotificationService
{
    public interface INotificationService
    {
        public Task AddNotification(Notification notification);
        public Task<IEnumerable<Notifications>> GetNotifications(int employeeId);
    }
}
