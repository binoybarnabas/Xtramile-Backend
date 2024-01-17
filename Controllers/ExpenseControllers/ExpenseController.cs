using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.ExpenseService;

namespace XtramileBackend.Controllers.ExpenseControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {

        private readonly IExpenseServices _expenseServices;

        public ExpenseController(IExpenseServices expenseServices) {

            _expenseServices = expenseServices;
        }

        [HttpGet]
        public IActionResult GetExpenses() {
             IEnumerable<TBL_EXPENSE> expenseData= _expenseServices.GetExpenses();
            return Ok(expenseData);
       
        }

        [HttpPost]
        public IActionResult AddExpense([FromBody] TBL_EXPENSE Expense)
        {
            _expenseServices.AddExpense(Expense);
            return Ok(Expense);

        }

    }
}
