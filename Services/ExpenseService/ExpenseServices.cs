using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.ExpenseService
{
    public class ExpenseServices : IExpenseServices
    {

        private readonly IUnitOfWork _unitOfWork;
        public ExpenseServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TBL_EXPENSE> GetExpenses()
        {
            var ExpenseData = _unitOfWork.ExpenseRepository.GetAll();
            return ExpenseData;
        }

        public void AddExpense(TBL_EXPENSE expense)
        {
            _unitOfWork.ExpenseRepository.Add(expense);
            _unitOfWork.Complete();
        }
    }
}
