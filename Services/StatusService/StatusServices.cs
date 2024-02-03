using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.StatusService
{
    public class StatusServices : IStatusServices
    {

        private readonly IUnitOfWork _unitOfWork;

        public StatusServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<IEnumerable<TBL_STATUS>> GetAllStatusAsync()
        {
            try
            {
                var status = _unitOfWork.StatusRepository.GetAllAsync();
                return status;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting all status: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task AddStatusAsync(TBL_STATUS status)
        {
            try
            {
                await _unitOfWork.StatusRepository.AddAsync(status);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding status: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }

        }
        /// <summary>
        /// Return the status Id using the status code
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns>Status ID</returns>
        public async Task<int> GetStatusIdByCode(string statusCode)
        {
            IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();

            int statusId = (from status in statusData
                            where status.StatusCode == statusCode
                            select status.StatusId).FirstOrDefault();

            return statusId;
        }

    }
}
