using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.TravelDocumentFileData
{
    public interface ITravelDocumentFileDataService
    {
        public Task<IEnumerable<TravelDocumentFileDataModel>> GetTravelDocumentFileDatasAsync();
        public Task AddTravelDocumentFileAsync(TravelDocumentFileDataModel TravelDocumentFileData);
        public Task<TravelDocumentFileDataModel> GetTravelDocumentFileByIdAsync(int id);
    }
}
