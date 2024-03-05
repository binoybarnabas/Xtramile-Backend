using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.TravelDocumentType
{
    public interface ITravelDocumentTypeService
    {
        public Task<IEnumerable<TravelDocumentTypeModel>> GetTravelDocumentTypesAsync();
        public Task AddTravelDocumentTypeAsync(TravelDocumentTypeModel TravelDocumentType);
    }
}
