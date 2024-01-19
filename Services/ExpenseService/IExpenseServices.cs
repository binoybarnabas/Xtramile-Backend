using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.ExpenseService
{
    public interface IExpenseServices
    {
        public Task<IEnumerable<TBL_EXPENSE>> GetExpensesAsync();
        public Task AddExpenseAsync(TBL_EXPENSE expense);
    }
}
