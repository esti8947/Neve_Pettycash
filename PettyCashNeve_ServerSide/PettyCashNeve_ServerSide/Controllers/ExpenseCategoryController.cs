using BL.Services;
using BL.Services.ExpenseCategoryService;
using Entities.Models_Dto.ExpenseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PettyCashNeve_ServerSide.Dto;

namespace PettyCashNeve_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet("getActiveAndInactiveExpenseCategoryAsync")]
        public async Task<IActionResult> GetActiveAndInactiveExpenseCategoryAsync()
        {
            var serviceResponse = await _expenseCategoryService.GetActiveAndInactiveExpenseCategoryAsync();
            return HandleResponse(serviceResponse);
        }

        [HttpPost("createExpenseCategory")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateExpenseCategory([FromBody] ExpenseCategoryDto expenseCategory)
        {
            var serviceResponse = await _expenseCategoryService.CreateExpenseCategory(expenseCategory);
            return HandleResponse(serviceResponse);
        }

        [HttpDelete("deleteExpenseCategory/{expenseCategoryId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteExpenseCategory(int expenseCategoryId)
        {
            var serviceResponse = await _expenseCategoryService.DeleteExpenseCategory(expenseCategoryId);
            return HandleResponse(serviceResponse);
        }

        [HttpGet("activateExpenseCategory/{expenseCategoryId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateExpenseCategory(int expenseCategoryId)
        {
            var serviceResponse = await _expenseCategoryService.ActivateExpenseCategory(expenseCategoryId);
            return HandleResponse(serviceResponse);
        }

        [HttpPut("updateExpenseCategory")]
        public async Task<IActionResult> UpdateExpenseCategory([FromBody] ExpenseCategoryDto updatedExpenseCategory)
        {
            var serviceResponse = await _expenseCategoryService.UpdateExpenseCategory(updatedExpenseCategory);
            return HandleResponse(serviceResponse);
        }

    }
}
