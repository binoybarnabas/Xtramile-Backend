using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.FinanceDepartment
{
    public class FinanceDepartmentService : IFinanceDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FinanceDepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// get all the requests to the finance department personnel
        /// get the requests data
        /// get the employees data
        /// get the travel types
        /// </summary>
        /// <returns>
        /// A list containing requestcode, firstname of employee, lastname of employee, email, request date, type of transportation
        /// </returns>
        public async Task<IEnumerable<FinanceRequest>> GetIncomingRequests()
        {
            try
            {
                IEnumerable<TBL_REQUEST> requests = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_TYPE> travelTypes = await _unitOfWork.TravelTypeRepository.GetAllAsync();

                var incomingRequestData = (from request in requests
                                           join employee in employees on request.CreatedBy equals employee.EmpId
                                           join travelType in travelTypes on request.TravelTypeId equals travelType.TravelTypeID
                                           select new FinanceRequest
                                           {
                                               RequestCode = request.RequestCode,
                                               FirstName = employee.FirstName,
                                               LastName = employee.LastName,
                                               Email = employee.Email,
                                               RequestDate = request.CreatedOn,
                                               TypeName = travelType.TypeName
                                           }).ToList();

                return incomingRequestData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting incoming requests: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// get all the informations for the requests
        /// </summary>
        /// <param name="sortField"></param>
        /// <param name="isDescending"></param>
        /// <returns>
        ///  A list containing requestcode, firstname of employee, lastname of employee, email, request date, type of transportation in sorted order
        ///  The sort can be done based on fields like request code, first name and date of request.
        /// </returns>
        public async Task<IEnumerable<FinanceRequest>> SortIncomingList(string sortField, bool isDescending)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requests = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_TYPE> travelTypes = await _unitOfWork.TravelTypeRepository.GetAllAsync();

                var incomingRequestData = (from request in requests
                                           join employee in employees on request.CreatedBy equals employee.EmpId
                                           join travelType in travelTypes on request.TravelTypeId equals travelType.TravelTypeID
                                           select new FinanceRequest
                                           {
                                               RequestCode = request.RequestCode,
                                               FirstName = employee.FirstName,
                                               LastName = employee.LastName,
                                               Email = employee.Email,
                                               RequestDate = request.CreatedOn,
                                               TypeName = travelType.TypeName
                                           });

                // Sort the data based on the input parameters
                switch (sortField.ToLowerInvariant())
                {
                    case "requestcode":
                        incomingRequestData = isDescending ? incomingRequestData.OrderByDescending(data => data.RequestCode) : incomingRequestData.OrderBy(data => data.RequestCode);
                        break;
                    case "firstname":
                        incomingRequestData = isDescending ? incomingRequestData.OrderByDescending(data => data.FirstName) : incomingRequestData.OrderBy(data => data.FirstName);
                        break;
                    case "date":
                        incomingRequestData = isDescending ? incomingRequestData.OrderByDescending(data => data.RequestDate) : incomingRequestData.OrderBy(data => data.RequestDate);
                        break;
                    default:
                        break;
                }

                return incomingRequestData.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting incoming requests: {ex.Message}");
                throw;
            }

        }

        /// <summary>
        /// A function to get all the information of invoices and expenses for travels.
        /// </summary>
        /// <returns>
        /// A list which contains InvoiceId, VendorName, VendorEmail, Amount, date of payment for a particular invoice
        /// </returns>

        public async Task<IEnumerable<InvoiceAttachment>> GetAllInvoiceAttachments()
        {
            try
            {
                IEnumerable<TBL_INVOICE> invoices = await _unitOfWork.InvoiceRepository.GetAllAsync();
                IEnumerable<TBL_EXPENSE> expenses = await _unitOfWork.ExpenseRepository.GetAllAsync();

                var invoiceAttachments = (from invoice in invoices
                                          join expense in expenses
                                                                     on invoice.InvoiceId equals expense.InvoiceId
                                          select new InvoiceAttachment
                                          {
                                              InvoiceId = invoice.InvoiceId,
                                              VendorName = expense.VendorName,
                                              VendorEmail = expense.VendorEmail,
                                              Amount = expense.InvoiceAmount,
                                              PaidDate = expense.PaidOn,
                                              UtrId = expense.UtrId
                                          });

                return invoiceAttachments.ToList();

            }

            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting incoming requests: {ex.Message}");
                throw;
            }

        }

        /// <summary>
        /// A function to update the status of payment based on invoice and to add UTRid for the corresponding transaction.
        /// </summary>
        /// <param name="InvoiceId"></param>
        /// <param name="invoiceStatus"></param>
        /// <returns>
        /// It updates the current data with the new UTR id and date of payment.
        /// </returns>
        public async Task<Boolean> UpdateInvoiceStatus(int InvoiceId, InvoiceStatus invoiceStatus)
        {
            try
            {
                IEnumerable<TBL_EXPENSE> existingExpenseStatusList = await _unitOfWork.ExpenseRepository.GetAllAsync();
                TBL_EXPENSE? existingExpense = (from expense in existingExpenseStatusList
                                                where expense.InvoiceId == InvoiceId
                                                select expense).FirstOrDefault();

                existingExpense.UtrId = invoiceStatus.UtrId;
                existingExpense.PaidOn = DateTime.Now;

                _unitOfWork.ExpenseRepository.Update(existingExpense);
                _unitOfWork.Complete();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting incoming requests: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsUtr"></param>
        /// string sortField, bool isDescending
        /// <returns></returns>
        public async Task<IEnumerable<InvoiceAttachment>> GetInvoicesBasedOnStatus(bool IsUtr)
        {
            try
            {
                IEnumerable<TBL_INVOICE> invoices = await _unitOfWork.InvoiceRepository.GetAllAsync();
                IEnumerable<TBL_EXPENSE> expenses = await _unitOfWork.ExpenseRepository.GetAllAsync();

                var invoiceAttachments = (from invoice in invoices
                                          join expense in expenses
                                          on invoice.InvoiceId equals expense.InvoiceId
                                          select new InvoiceAttachment
                                          {
                                              InvoiceId = invoice.InvoiceId,
                                              VendorName = expense.VendorName,
                                              VendorEmail = expense.VendorEmail,
                                              Amount = expense.InvoiceAmount,
                                              PaidDate = expense.PaidOn,
                                              UtrId = expense.UtrId
                                          });

                if (IsUtr)
                {
                    var paidDetails = invoiceAttachments.Where((invoiceAttachment) => invoiceAttachment.UtrId != null);
                    return paidDetails.ToList();
                }
                else if (IsUtr == false)
                {
                    var notPaidDetails = invoiceAttachments.Where((invoiceAttachment) => invoiceAttachment.UtrId == null);
                    return notPaidDetails.ToList();
                }
                else
                    return invoiceAttachments.ToList();

            }

            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting incoming requests: {ex.Message}");
                throw;
            }

        }
    }
}