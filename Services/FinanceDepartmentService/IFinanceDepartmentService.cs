using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Services.FinanceDepartment
{
    public interface IFinanceDepartmentService
    {
        public Task<IEnumerable<FinanceRequest>> GetIncomingRequests();
        public Task<IEnumerable<FinanceRequest>> SortIncomingList(string sortField, bool isDescending);
    }
}
