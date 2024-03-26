using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XtramileBackend.Services.NotificationService
{
    public class NotifcationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public NotifcationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddNotification(Notification notification)
        {
            try
            {
                await _unitOfWork.NotificationRepository.AddAsync(notification);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding an available option: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }

        }

        public async Task<IEnumerable<Notifications>> GetNotifications(int employeeId) {
            try {
                IEnumerable<Notification> notificationData = await _unitOfWork.NotificationRepository.GetAllAsync();
                IEnumerable<Notifications> notifications = notificationData
                                                           .Where((data) => data.EmployeeId == employeeId)
                                                           .Select(data => new Notifications
                                                           {
                                                               Date = data.CreatedOn.ToString("dd-MM-yyyy"),
                                                               Message = data.NotificationBody.ToString(),
                                                               Time = data.CreatedOn.ToString("HH:mm:ss tt")
                                                           }
                                                           );
                return notifications;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding an available option: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
