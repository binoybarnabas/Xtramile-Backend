using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.InvoiceService
{
    public class InvoiceServices : IInvoiceServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<TBL_INVOICE>> GetInvoicesAsync()
        {
            try
            {
                var invoiceData = await _unitOfWork.InvoiceRepository.GetAllAsync();
                return invoiceData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting priorities: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task AddInvoiceAsync(TBL_INVOICE invoice)
        {
            try
            {
                await _unitOfWork.InvoiceRepository.AddAsync(invoice);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding an invoice: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
