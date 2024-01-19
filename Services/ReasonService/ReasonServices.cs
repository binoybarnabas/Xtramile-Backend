using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.ReasonService
{
    public class ReasonServices : IReasonServices
    {

        private readonly IUnitOfWork _unitOfWork;

        public ReasonServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<IEnumerable<TBL_REASON>> GetAllReasonsAsync()
        {
            try
            {
                var reasons = _unitOfWork.ReasonRepository.GetAllAsync();
                return reasons;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting reasons: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }

        }

        public async Task AddReasonAsync(TBL_REASON reason)
        {
            try
            {
                await _unitOfWork.ReasonRepository.AddAsync(reason);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding a reason: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }

        }
    }
}
