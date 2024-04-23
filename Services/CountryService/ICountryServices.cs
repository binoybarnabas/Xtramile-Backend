using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.CountryService
{
    public interface ICountryServices
    {
        Task<IEnumerable<Country>> GetCountriesAsync();
        Task AddCountryAsync(Country country);
    }
}
