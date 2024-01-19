using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.CountryService
{
    public interface ICountryServices
    {
        Task<IEnumerable<TBL_COUNTRY>> GetCountriesAsync();
        Task AddCountryAsync(TBL_COUNTRY country);
    }
}
