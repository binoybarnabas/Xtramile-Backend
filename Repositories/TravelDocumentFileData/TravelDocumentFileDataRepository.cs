using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.TravelDocumentFileData
{
    public class TravelDocumentFileDataRepository : Repository<TravelDocumentFileDataModel>, ITravelDocumentFileDataRepository
    {
        public TravelDocumentFileDataRepository(AppDBContext dbContext) : base(dbContext) { }
    }
}
