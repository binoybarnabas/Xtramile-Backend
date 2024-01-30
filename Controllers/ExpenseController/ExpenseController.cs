using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.ExpenseService;

namespace XtramileBackend.Controllers.ExpenseControllers
{
    [Route("api/expense")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseServices _expenseServices;

        public ExpenseController(IExpenseServices expenseServices)
        {
            _expenseServices = expenseServices;
        }

        [HttpGet("expenses")]
        public async Task<IActionResult> GetPrioritiesAsync()
        {
            try
            {
                IEnumerable<TBL_EXPENSE> expenseData = await _expenseServices.GetExpensesAsync();
                return Ok(expenseData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting expenses: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddExpenseAsync([FromBody] TBL_EXPENSE expense)
        {
            try
            {
                await _expenseServices.AddExpenseAsync(expense);
                return Ok(expense);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding an expense: {ex.Message}");
            }
        }
    }
}
