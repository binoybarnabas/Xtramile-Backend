using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.ExpenseRepository
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {

        public ExpenseRepository(AppDBContext dbContext) : base(dbContext){ 
       
        }

    }
}
