using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.ExpenseService
{
    public interface IExpenseServices
    {
        public Task<IEnumerable<Expense>> GetExpensesAsync();
        public Task AddExpenseAsync(Expense expense);
    }
}
