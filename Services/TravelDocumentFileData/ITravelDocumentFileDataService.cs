using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.TravelDocumentFileData
{
    public interface ITravelDocumentFileDataService
    {
        public Task<IEnumerable<TravelDocumentFileDataModel>> GetTravelDocumentFileDatasAsync();
        public Task AddTravelDocumentTypeAsync(TravelDocumentFileDataModel TravelDocumentFileData);
    }
}
