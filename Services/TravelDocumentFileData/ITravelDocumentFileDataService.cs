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
        public Task<PageinatedResult<TravelDocumentViewModel>> GetFilteredDocumentsOnTAScreen(string fileType, HttpContext httpContext, int pageNumber, int itemsPerPage);
        public Task<PageinatedResult<TravelDocumentViewModel>> GetExpiredDocuments(string fileType, HttpContext httpContext, int pageNumber, int itemsPerPage);
        public Task DeleteTravelDocument(int FileId);
        public Task<PageinatedResult<TravelDocumentViewModel>> GetValidDocuments(string fileType, HttpContext httpContext, int pageNumber, int itemsPerPage);
        public Task<RelevantDocument> GetAllRelevantDocuments(int requestId, HttpContext httpContext);
        public Task<IEnumerable<TravelDocumentViewModel>> GetTravelDocumentByEmployeeName(string filetype, string employeeName, int filterId, HttpContext httpContext);

    }
}
