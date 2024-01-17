using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.InvoiceService
{
    public class InvoiceServices : IInvoiceServices
    {

        private readonly IUnitOfWork _unitOfWork;

        public InvoiceServices(IUnitOfWork unitOfWork) { 
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TBL_INVOICE> GetAllInvoices()
        {
            var InvoiceData = _unitOfWork.InvoiceRepository.GetAll();
            return InvoiceData;
        }

        public void AddInvoice(TBL_INVOICE invoice) { 
            _unitOfWork.InvoiceRepository.Add(invoice);
            _unitOfWork.Complete();
        }
    }
}
