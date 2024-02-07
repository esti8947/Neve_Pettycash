using BL.Services.MonthlyBudgetService;
using Entities.Models_Dto.BudgetDto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PettyCashNeve_ServerSide.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class MonthlyBudgetController : BaseController
    {
        private readonly IMonthlyBudgetService _monthlyBudgetService; // Change the service interface

        public MonthlyBudgetController(IMonthlyBudgetService monthlyBudgetService) // Change the constructor parameter
        {
            _monthlyBudgetService = monthlyBudgetService;
        }

        [HttpGet("getMonthlyBudgetsByDepartmentId/{departmentId}")]
        public async Task<IActionResult> GetMonthlyBudgetsByDepartmentId(int departmentId)
        {
            var serviceResponse = await _monthlyBudgetService.GetMonthlyBudgetsByDepartmentIdAsync(departmentId); 
            return HandleResponse(serviceResponse);
        }

        [HttpGet("getMonthlyBudgetsByDepartmentIdAndIsActive/{departmentId}")]
        public async Task<IActionResult> GetMonthlyBudgetsByDepartmentIdAndIsActive(int departmentId)
        {
            var serviceResponse = await _monthlyBudgetService.GetMonthlyBudgetByDepartmentIdAndIsActiveAsync(departmentId); 
            return HandleResponse(serviceResponse);
        }

        [HttpPost("createMonthlyBudget")]
        public async Task<IActionResult> CreateMonthlyBudget([FromBody] MonthlyBudgetDto monthlyBudgetDto)
        {
            var serviceResponse = await _monthlyBudgetService.CreateMonthlyBudgetAsync(monthlyBudgetDto); 
            return HandleResponse(serviceResponse);
        }

        [HttpPost("addSumToMonthlyBudgetCeiling/{monthlyBudgetId}/{additionalAmount}")]
        public async Task<IActionResult> AddSumToMonthlyBudgetCeiling(int monthlyBudgetId, int additionalAmount)
        {
            var serviceResponse = await _monthlyBudgetService.AddSumToMonthlyBudgetCeilingAsync(monthlyBudgetId, additionalAmount); 
            return HandleResponse(serviceResponse);
        }

        [HttpDelete("deleteMonthlyBudget/{monthlyBudgetId}")]
        public async Task<IActionResult> DeleteMonthlyBudget(int monthlyBudgetId)
        {
            var serviceResponse = await _monthlyBudgetService.DeleteMonthlyBudgetAsync(monthlyBudgetId); 
            return HandleResponse(serviceResponse);
        }
    }
}
