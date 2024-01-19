using System.Diagnostics.Metrics;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.PerdiumService
{
    public class PerdiumServices : IPerdiumServices
    {
        private readonly IUnitOfWork _unitOfWork;
  
        public PerdiumServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); ;
        }

        public async Task<IEnumerable<TBL_PER_DIUM>> GetPerdiumAsync()
        {
            try
            {
                return await Task.Run(() => _unitOfWork.PerdiumRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting perdium: {ex.Message}");
                throw;
            }
        }

        public async Task AddPerdiumAsync(TBL_PER_DIUM perdium)
        {
            try
            {
                await _unitOfWork.PerdiumRepository.AddAsync(perdium);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding perdium: {ex.Message}");
                throw;
            }
        }
    }
}
