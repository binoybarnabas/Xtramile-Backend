using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Services.TravelAdminService
{
    public interface ITravelAdminService
    {
        public Task<OngoingTravelAdminPaged> OnGoingTravel(int pageSize, int pageIndex);
        public Task<RequestTableViewTravelAdminPaged> GetIncomingRequests(int pageIndex, int pageSize);
        public Task<OptionCard> GetSelectedOptionFromEmployee(int reqId);
        public Task<RequestTableViewTravelAdminPaged> GetTravelRequests(string statusCode,int pageSize,int pageIndex);

        public Task<TravelRequestEmployeeViewModel> GetEmployeeRequestDetail(int requestId);

        public Task<RequestTableViewTravelAdminPaged> GetIncomingRequestsSorted(int pageIndex, int pageSize, bool employeeName, bool date);

        public Task<IEnumerable<RequestTableViewTravelAdmin>> GetEmployeeRequestsByDateAsync(string date);

        public  Task<IEnumerable<RequestTableViewTravelAdmin>> GetEmployeeRequestsByEmployeeNameAsync(string employeeName);
        public Task<byte[]> GenerateModeCountFromMonthReport(string monthName);
        public Task<byte[]> GenerateModeCountFromProjectIdExcelReport(int projectId);
        public Task<Dictionary<string, int>> GetRequestsByMonth();
        public Task<byte[]> GenerateReportForMonth(string monthName);
        public Task<int?> GetSelectedTravelOptionFromEmployee(int reqId);

    }
}
