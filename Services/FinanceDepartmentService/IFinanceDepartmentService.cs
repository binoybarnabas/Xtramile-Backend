using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Services.FinanceDepartment
{
    public interface IFinanceDepartmentService
    {
        public Task<IEnumerable<FinanceRequest>> GetIncomingRequests();
        public Task<IEnumerable<FinanceRequest>> SortIncomingList(string sortField, bool isDescending);

        public Task<IEnumerable<InvoiceAttachment>> GetAllInvoiceAttachments();
        public Task<Boolean> UpdateInvoiceStatus(int InvoiceId, InvoiceStatus invoiceStatus);
        public Task<IEnumerable<InvoiceAttachment>> GetInvoicesBasedOnStatus(bool IsUtr);
    }
}
