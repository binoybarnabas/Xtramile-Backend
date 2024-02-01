using System.Diagnostics.Metrics;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.PerdiemService
{
    public class PerdiemServices : IPerdiemServices
    {
        private readonly IUnitOfWork _unitOfWork;
  
        public PerdiemServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); ;
        }

        public async Task<IEnumerable<TBL_PER_DIEM>> GetPerdiemAsync()
        {
            try
            {
                return await Task.Run(() => _unitOfWork.PerdiemRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting perdiem: {ex.Message}");
                throw;
            }
        }

        public async Task AddPerdiemAsync(TBL_PER_DIEM perdiem)
        {
            try
            {
                await _unitOfWork.PerdiemRepository.AddAsync(perdiem);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding perdiem: {ex.Message}");
                throw;
            }
        }
    }
}
