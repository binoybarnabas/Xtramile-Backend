using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.CountryRepository
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(AppDBContext dbContext) : base(dbContext)
        {

        }
    }
}
