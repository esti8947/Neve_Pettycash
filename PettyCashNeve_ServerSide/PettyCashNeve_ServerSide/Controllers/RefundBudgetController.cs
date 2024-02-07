using BL.Services.RefundBudgetService;
using Entities.Models_Dto.BudgetDto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PettyCashNeve_ServerSide.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class RefundBudgetController : BaseController
    {
        private readonly IRefundBudgetService _refundBudgetService;

        public RefundBudgetController(IRefundBudgetService refundBudgetService) 
        {
            _refundBudgetService = refundBudgetService;
        }

        [HttpGet("getRefundBudgetsByDepartmentId/{departmentId}")]
        public async Task<IActionResult> GetRefundBudgetsByDepartmentId(int departmentId)
        {
            var serviceResponse = await _refundBudgetService.GetRefundBudgetsByDepartmentIdAsync(departmentId); 
            return HandleResponse(serviceResponse);
        }

        [HttpGet("getRefundBudgetsByDepartmentIdAndIsActive/{departmentId}")]
        public async Task<IActionResult> GetRefundBudgetsByDepartmentIdAndIsActive(int departmentId)
        {
            var serviceResponse = await _refundBudgetService.GetRefundBudgetByDepartmentIdAndIsActiveAsync(departmentId); 
            return HandleResponse(serviceResponse);
        }

        [HttpPost("createRefundBudget")]
        public async Task<IActionResult> CreateRefundBudget([FromBody] RefundBudgetDto refundBudgetDto)
        {
            var serviceResponse = await _refundBudgetService.CreateRefundBudgetAsync(refundBudgetDto);
            return HandleResponse(serviceResponse);
        }


        [HttpDelete("deleteRefundBudget/{refundBudgetId}")]
        public async Task<IActionResult> DeleteRefundBudget(int refundBudgetId)
        {
            var serviceResponse = await _refundBudgetService.DeleteRefundBudgetAsync(refundBudgetId); 
            return HandleResponse(serviceResponse);
        }
    }
}
