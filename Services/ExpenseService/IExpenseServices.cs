using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.ExpenseService
{
    public interface IExpenseServices
    {
        public IEnumerable<TBL_EXPENSE> GetExpenses();
        public void AddExpense(TBL_EXPENSE expense);
    }
}
