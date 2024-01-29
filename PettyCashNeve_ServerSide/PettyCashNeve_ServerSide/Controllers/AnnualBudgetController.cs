using BL.Services.AnnualBudgetService;
using Entities.Models_Dto.BudgetDto;
using Microsoft.AspNetCore.Mvc;

namespace PettyCashNeve_ServerSide.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class AnnualBudgetController : BaseController
    {
        private readonly IAnnualBudgetService _annualBudgetService; 
        public AnnualBudgetController(IAnnualBudgetService annualBudgetService) 
        {
            _annualBudgetService = annualBudgetService;
        }

        [HttpGet("getAnnualBudgetsByDepartmentId/{departmentId}")]
        public async Task<IActionResult> GetAnnualBudgetsByDepartmentId(int departmentId)
        {
            var serviceResponse = await _annualBudgetService.GetAnnualBudgetsByDepartmentIdAsync(departmentId);
            return HandleResponse(serviceResponse);
        }

        [HttpGet("getAnnualBudgetsByDepartmentIdAndIsActive/{departmentId}/{isActive}")]
        public async Task<IActionResult> GetAnnualBudgetsByDepartmentIdAndIsActive(int departmentId, bool isActive)
        {
            var serviceResponse = await _annualBudgetService.GetAnnualBudgetByDepartmentIdAndIsActiveAsync(departmentId); 
            return HandleResponse(serviceResponse);
        }

        [HttpPost("createAnnualBudget")]
        public async Task<IActionResult> CreateAnnualBudget([FromBody] AnnualBudgetDto annualBudgetDto)
        {
            var serviceResponse = await _annualBudgetService.CreateAnnualBudgetAsync(annualBudgetDto);
            return HandleResponse(serviceResponse);
        }

        [HttpPost("addSumToAnnualBudgetCeiling/{annualBudgetId}/{additionalAmount}")]
        public async Task<IActionResult> AddSumToAnnualBudgetCeiling(int annualBudgetId, int additionalAmount)
        {
            var serviceResponse = await _annualBudgetService.AddSumToAnnualBudgetCeilingAsync(annualBudgetId, additionalAmount);
            return HandleResponse(serviceResponse);
        }

        [HttpDelete("deleteAnnualBudget/{annualBudgetId}")]
        public async Task<IActionResult> DeleteAnnualBudget(int annualBudgetId)
        {
            var serviceResponse = await _annualBudgetService.DeleteAnnualBudgetAsync(annualBudgetId);
            return HandleResponse(serviceResponse);
        }
    }
}
