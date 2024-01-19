using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.CountryService
{
    public class CountryServices : ICountryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public CountryServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); ;
        }

        public async Task<IEnumerable<TBL_COUNTRY>> GetCountriesAsync()
        {
            try
            {
                return await Task.Run(() => _unitOfWork.CountryRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting countries : {ex.Message}");
                throw;
            }
        }

        public async Task AddCountryAsync(TBL_COUNTRY country)
        {
            try
            {
                await _unitOfWork.CountryRepository.AddAsync(country);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding countries : {ex.Message}");
                throw;
            }
        }
    }
}
