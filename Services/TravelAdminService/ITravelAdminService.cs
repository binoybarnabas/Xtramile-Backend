﻿using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Services.TravelAdminService
{
    public interface ITravelAdminService
    {
        public Task<OngoingTravelAdminPaged> OnGoingTravel(int pageIndex,int pageSize);
        public Task<RequestTableViewTravelAdminPaged> GetIncomingRequests(int pageIndex, int pageSize);
        public Task<OptionCard> GetSelectedOptionFromEmployee(int reqId);
        public Task<RequestTableViewTravelAdminPaged> GetTravelRequests(string primaryStatusCode, string secondaryStatusCode, int pageSize,int pageIndex);

        public Task<TravelRequestEmployeeViewModel> GetEmployeeRequestDetail(int requestId);

        public Task<RequestTableViewTravelAdminPaged> GetIncomingRequestsSorted(int pageIndex, int pageSize, bool employeeName, bool date);

        public Task<IEnumerable<RequestTableViewTravelAdmin>> GetEmployeeRequestsByDateAsync(string date);

        public  Task<IEnumerable<RequestTableViewTravelAdmin>> GetEmployeeRequestsByEmployeeNameAsync(string employeeName);
        public Task<byte[]> GenerateModeCountFromMonthReport(string monthName);
        public Task<byte[]> GenerateModeCountFromProjectIdExcelReport(int projectId);
        public Task<Dictionary<string, int>> GetRequestsByMonth();
        public Task<byte[]> GenerateReportForMonthAndYear(string monthName,int year);
        public Task<int?> GetSelectedTravelOptionFromEmployee(int reqId);
        public Task<IEnumerable<CompletedTripsCard>> GetCompletedTrips(int empId);
        public Task<ClosedTravelAdminPaged> ClosedTravel(int pageIndex, int pageSize);
    }
}
