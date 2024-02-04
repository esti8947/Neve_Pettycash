using BL.Services.ExpenseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PettyCashNeve_ServerSide.Services.MonthlyCashRegisterService;

namespace PettyCashNeve_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseMoreInfoController : BaseController
    {
        private readonly IExpenseMoreInfoService _expenseMoreInfoService;
        private readonly IMonthlyCashRegisterService _monthlyCashRegisterService;
        public ExpenseMoreInfoController(IExpenseMoreInfoService expenseMoreInfoService, IMonthlyCashRegisterService monthlyCashRegisterService)
        {
            _expenseMoreInfoService = expenseMoreInfoService;
            _monthlyCashRegisterService = monthlyCashRegisterService;
        }

        //[HttpGet("getExpenseReportInfo")]
        //public async Task<IActionResult> GetExpenseReportInfo()
        //{
        //    var serviceResponse = await _expenseMoreInfoService.GetExpenseReportInfoAsync();
        //    return HandleResponse(serviceResponse);
        //}

        //[HttpGet("getExpensesReportOfCurrentMonth")]
        //[Authorize]
        //public async Task<IActionResult> GetExpensesReportOfCurrentMonth()
        //{
        //    try
        //    {
        //        var userId = UserId;

        //        if (string.IsNullOrEmpty(userId))
        //        {
        //            return BadRequest("UserId is null or empty.");
        //        }
        //        var currentMonth = await _monthlyCashRegisterService.GetCurrentMonthByUserIdAsync(userId);
        //        var serviceResponse = await _expenseMoreInfoService.GetExpensesReportOfCurrentMonth(userId, currentMonth);
        //        return HandleResponse(serviceResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}

        [HttpGet("getExpensesReportOfUser")]
        [Authorize]
        public async Task<IActionResult> GetExpensesReportOfUser()
        {
            try
            {
                var userId = UserId;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserId is null or empty.");
                }
                var serviceResponse = await _expenseMoreInfoService.GetExpensesReportOfUser(userId);
                return HandleResponse(serviceResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("getExpensesByYearAndMonth/{year}/{month}")]
        [Authorize]
        public async Task<IActionResult> GetExpensesByYearAndMonth(int year, int month, [FromQuery] int? departmentId = null)
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
                    var serviceResponse = await _expenseMoreInfoService.GetExpensesOfDepartmentByYearAndMonth(departmentId.Value, year, month);
                    return HandleResponse(serviceResponse);
                }
                else
                {
                    var serviceResponse = await _expenseMoreInfoService.GetExpensesByYearAndMonth(userId, year, month);
                    return HandleResponse(serviceResponse);
                }

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getUnlockedExpenses/{departmentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUnlockedExpenses(int departmentId)
        {
            var serviceResponse = await _expenseMoreInfoService.GetUnlockedExpenses(departmentId);
            return HandleResponse(serviceResponse);
        }


    }
}
