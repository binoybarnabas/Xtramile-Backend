using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.NotificationRepository
{
    public class NotificationRepository:Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDBContext context):base(context) { 
        }
    }
}
