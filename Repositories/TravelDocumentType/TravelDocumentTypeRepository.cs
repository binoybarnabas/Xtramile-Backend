using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.TravelDocumentType
{
    public class TravelDocumentTypeRepository : Repository<TravelDocumentTypeModel>, ITravelDocumentTypeRepository
    {
        public TravelDocumentTypeRepository(AppDBContext dbContext) : base(dbContext) { }
    }
}
