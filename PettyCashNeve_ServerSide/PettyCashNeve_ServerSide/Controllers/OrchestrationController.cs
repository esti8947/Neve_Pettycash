using BL.Services.EventService;
using BL.Services.ExpenseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PettyCashNeve_ServerSide.Services.MonthlyCashRegisterService;

namespace PettyCashNeve_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrchestrationController : BaseController
    {
        private readonly IEventService _eventService;
        private readonly IExpenseService _expenseService;
        private readonly IMonthlyCashRegisterService _monthlyCashRegisterService;

        public OrchestrationController(IEventService eventService, IExpenseService expenseService, IMonthlyCashRegisterService monthlyCashRegisterService)
        {
            _eventService = eventService;
            _expenseService = expenseService;
            _monthlyCashRegisterService = monthlyCashRegisterService;
        }

        [HttpGet("closeMonthlyActivities/{year}/{month}")]
        public async Task<IActionResult> CloseMonthlyActivities(int year, int month)
        {
            try
            {
                var userId = UserId;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserId is null or empty.");
                }

                var deactivateEventsResponse = await _eventService.DeactivateAllEvents();
                var approveExpensesResponse = await _expenseService.ApproveAllExpenses(userId, year, month);
                var deactivateMonthlyCashRegisterResponse = await _monthlyCashRegisterService.DeactivateMonthlyCashRegister(UserId);

                // Check the responses and handle accordingly
                if (deactivateEventsResponse.Success && approveExpensesResponse.Success && deactivateMonthlyCashRegisterResponse.Success)
                {
                    return HandleResponses(deactivateEventsResponse, approveExpensesResponse, deactivateMonthlyCashRegisterResponse);
                }
                else
                {
                    // Handle other scenarios if needed
                    return BadRequest("One or more actions failed.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

