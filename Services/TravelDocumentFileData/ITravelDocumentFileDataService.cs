using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.TravelDocumentFileData
{
    public interface ITravelDocumentFileDataService
    {
        public Task<IEnumerable<TravelDocumentFileDataModel>> GetTravelDocumentFileDatasAsync();
        public Task<TravelDocumentFileDataModel> AddTravelDocumentFileAsync(TravelDocument travelDocFile, HttpContext httpContext);
        public Task<TravelDocumentFileDataModel> GetTravelDocumentFileByIdAsync(int id);
        public Task<IEnumerable<TravelDocumentViewModel>> GetDocumentDetailOnEmployeeScreen(int employeeId, HttpContext httpContext);
        public Task<IEnumerable<TravelDocumentViewModel>> GetDocumentsOnTravelAdminScreen(HttpContext httpContext);
        public Task<IEnumerable<TravelDocumentViewModel>> GetFilteredDocumentsOnTAScreen(string fileType, HttpContext httpContext);
        public Task<IEnumerable<TravelDocumentViewModel>> GetExpiredDocuments(string fileType, HttpContext httpContext);
        public Task DeleteTravelDocument(int FileId);

    }
}
