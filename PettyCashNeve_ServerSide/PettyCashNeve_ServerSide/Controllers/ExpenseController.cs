using BL.Services.ExpenseService;
using Entities.Models_Dto.ExpenseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace PettyCashNeve_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpenseController : BaseController
    {
        private readonly IExpenseService _expenseService;
        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }


        [HttpGet("getAllExpenses")]
        public async Task<IActionResult> GetAllExpenses()
        {
            var serviceResponse = await _expenseService.GetAllExpensesAsync();
            return HandleResponse(serviceResponse);
        }

        [HttpGet("getExpensesOfUser")]
        public async Task<IActionResult> GetExpensesOfUser()
        {
            var userId = UserId;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is null or empty.");
            }
            var serviceResponse = await _expenseService.GetExpensesOfUserAsync(userId);
            return HandleResponse(serviceResponse);

        }

        [HttpGet("getExpensesOfUserByYear/{year}")]
        public async Task<IActionResult> GetExpensesOfUserByYear(int year, [FromQuery] int? departmentId = null)
        {
            try
            {
                var userId = UserId;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserId is null or empty.");
                }
                bool isManager = User.IsInRole("Admin");

                if (isManager && departmentId.HasValue)
                {
                    // Manager can specify a departmentId
                    var serviceResponse = await _expenseService.GetExpensesOfDepartmentByYear(departmentId.Value, year);
                    return HandleResponse(serviceResponse);
                }
                else
                {
                    // Use the userId for non-manager users or when no departmentId is specified
                    var serviceResponse = await _expenseService.GetExpensesOfUserByYear(userId, year);
                    return HandleResponse(serviceResponse);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("getUnapprovedExpensesByUserAsync")]
        public async Task<IActionResult> GetUnapprovedExpensesByUserAsync()
        {
            var userId = UserId;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is null or empty.");
            }
            var serviceResponse = await _expenseService.GetUnapprovedExpensesByUserAsync(userId);
            return HandleResponse(serviceResponse);

        }

        [HttpGet("getApprovedAndUnlockedExpensesOfDepartment/{departmentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetApprovedAndUnlockedExpensesOfDepartment(int departmentId)
        {
            var serviceResponese = await _expenseService.GetApprovedAndUnlockedExpensesOfDepartment(departmentId);
            return HandleResponse(serviceResponese);
        }
        [HttpGet("getExpensesByDepartment")]
        public async Task<IActionResult> GetExpensesByDepartment()
        {
            // Extract the department ID claim from the authenticated user's claims
            var departmentClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimaryGroupSid);

            if (departmentClaim != null && int.TryParse(departmentClaim.Value, out int departmentId))
            {
                var serviceResponse = await _expenseService.GetExpensesOfDepartmentAsync(departmentId);
                return HandleResponse(serviceResponse);

            }
            else
            {
                return Unauthorized("User not authenticated");
            }
        }

        [HttpPut("updateExpense")]
        public async Task<IActionResult> UpdateExpense([FromBody] ExpenseDto updatedExpense)
        {
            try
            {
                var userId = UserId;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserId is null or empty.");
                }

                // Your code to create expense using userId
                updatedExpense.UpdatedBy = userId;
                    var serviceResponse = await _expenseService.UpdateExpenseAsync(updatedExpense);
                    return HandleResponse(serviceResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpDelete("deleteExpense/{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var serviceResponse = await _expenseService.DeleteExpenseAsync(id);
                    return HandleResponse(serviceResponse);
                }
                else
                {
                    return Unauthorized("User not authenticated.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("createExpense")]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseDto newExpense)
        {
            try
            {
                var userId = UserId;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserId is null or empty.");
                }

                // Your code to create expense using userId
                newExpense.UpdatedBy = userId;

                    var serviceResponse = await _expenseService.CreateExpenseAsync(newExpense);
                    return HandleResponse(serviceResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("approveExpenses")]
        public async Task<IActionResult> ApproveExpenses([FromBody] List<ExpenseDto> expensesList)
        {
            var serviceResponse = await _expenseService.ApproveExpenses(expensesList);
            return HandleResponse(serviceResponse);
        }

        [HttpGet("approveAllExpenses/{year}/{month}")]
        
        public async Task<IActionResult> ApproveAllExpenses(int year, int month)
        {
            var userId = UserId;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is null or empty.");
            }
            var serviceResponse = await _expenseService.ApproveAllExpenses(userId, year, month);
            return HandleResponse(serviceResponse);
        }

        [HttpGet("lockExpenses/{month}/{year}/{departmentId}")]
        public async Task<IActionResult> LockExpenses(int month, int year, int departmentId)
        {
            var serviceResponse = await _expenseService.LockExpenses(month, year, departmentId);
            return HandleResponse(serviceResponse);
        }

        [HttpGet("getExpensesAmountForMonth/{month}/{year}")]
        public async Task<IActionResult> GetExpensesAmountForMonth(int month, int year)
        {
            var userId = UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is null or empty.");
            }
            var serviceResponse = await _expenseService.GetExpensesAmountForMonth(month, year, userId);
            return HandleResponse(serviceResponse);
        }
    }
}
