using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Utils
{
    public class NotifcationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public NotifcationService(IUnitOfWork unitOfWork) { 
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
    }
}
