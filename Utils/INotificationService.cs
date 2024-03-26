using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Utils
{
    public interface INotificationService
    {
        public Task AddNotification(Notification notification);
    }
}
