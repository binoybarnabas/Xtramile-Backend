using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.ExpenseRepository
{
    public class ExpenseRepository : Repository<TBL_EXPENSE>, IExpenseRepository
    {

        public ExpenseRepository(AppDBContext dbContext) : base(dbContext){ 
       
        }

    }
}
