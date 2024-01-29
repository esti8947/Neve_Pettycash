using BL.Services;
using BL.Services.ExpenseCategoryService;
using Entities.Models_Dto.ExpenseDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PettyCashNeve_ServerSide.Dto;

namespace PettyCashNeve_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseCategoryController : BaseController
    {
        private readonly IExpenseCategoryService _expenseCategoryService;
        public ExpenseCategoryController(IExpenseCategoryService expenseCategoryService)
        {
            _expenseCategoryService = expenseCategoryService;
        }

        [HttpGet("getAllExpensesCategory")]
        public async Task<IActionResult> GetAllExpensesCategory()
        {
            var serviceResponse = await _expenseCategoryService.GetAllExpenseCategoriesAsync();
            return HandleResponse(serviceResponse);
        }

        [HttpPost("createExpenseCategory")]
        public async Task<IActionResult> CreateExpenseCategory([FromBody] ExpenseCategoryDto expenseCategory)
        {
            var serviceResponse = await _expenseCategoryService.CreateExpenseCategory(expenseCategory);
            return HandleResponse(serviceResponse);
        }

        [HttpDelete("deleteExpenseCategory/{expenseCategoryId}")]
        public async Task<IActionResult> DeleteExpenseCategory(int expenseCategoryId)
        {
            var serviceResponse = await _expenseCategoryService.DeleteExpenseCategory(expenseCategoryId);
            return HandleResponse(serviceResponse);
        }

    }
}
