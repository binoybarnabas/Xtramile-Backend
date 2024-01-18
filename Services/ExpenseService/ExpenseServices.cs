using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.ExpenseService
{
    public class ExpenseServices : IExpenseServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpenseServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<TBL_EXPENSE>> GetExpensesAsync()
        {
            try
            {
                var expenseData = await _unitOfWork.ExpenseRepository.GetAllAsync();
                return expenseData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting expenses: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task AddExpenseAsync(TBL_EXPENSE expense)
        {
            try
            {
                await _unitOfWork.ExpenseRepository.AddAsync(expense);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding an expense: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
