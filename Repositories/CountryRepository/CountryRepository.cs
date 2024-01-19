using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.CountryRepository
{
    public class CountryRepository : Repository<TBL_COUNTRY>, ICountryRepository
    {
        public CountryRepository(AppDBContext dbContext) : base(dbContext)
        {

        }
    }
}
