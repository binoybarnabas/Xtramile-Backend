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
        public FinanceDepartmentService(IUnitOfWork unitOfWork) { 
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
        public async Task<IEnumerable<FinanceRequest>> GetIncomingRequests() {
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
        /// 
        /// </summary>
        /// <param name="sortField"></param>
        /// <param name="isDescending"></param>
        /// <returns>
        ///  A list containing requestcode, firstname of employee, lastname of employee, email, request date, type of transportation in sorted order
        ///  The sort can be done based on fields like request code, first name and date of request.
        /// </returns>
        public async Task<IEnumerable<FinanceRequest>> SortIncomingList(string sortField, bool isDescending) {
            try { 
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
            catch (Exception ex) {
                Console.WriteLine($"An error occurred while getting incoming requests: {ex.Message}");
                throw;
            }

        }

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
                                                                         PaidDate = expense.PaidOn
                                                                     });
                
                return invoiceAttachments.ToList();

            }

            catch (Exception ex) {
                Console.WriteLine($"An error occurred while getting incoming requests: {ex.Message}");
                throw;
            }
            
        
          
        }



    }
}
