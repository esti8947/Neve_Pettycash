using BL.Services;
using BL.Services.BudgetTypeService;
using Entities.Models_Dto.BudgetDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PettyCashNeve_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BudgetTypeController : BaseController
    {
        private readonly IBudgetTypeService _budgetTypeService;
        public BudgetTypeController(IBudgetTypeService budgetTypeService)
        {
            _budgetTypeService = budgetTypeService;
        }

        [HttpGet("getBudgetTypeList")]
        public async Task<IActionResult> GetBudgetTypeList()
        {
            var serviceResponse = await _budgetTypeService.GetBudgetTypes();
            return HandleResponse(serviceResponse);
        }

        [HttpGet("getBudgetTypeById/{id}")]
        public async Task<IActionResult> GetBudgetTypeById(int id)
        {
            var serviceResponse = await _budgetTypeService.GetBudgetTypeByIdAsync(id);
            return HandleResponse(serviceResponse);
        }
        [HttpGet("getBudgetInformation")]
        public async Task<IActionResult> GetBudgetInformation([FromQuery] int? departmentIdFromAdmin = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                bool isAdmin = User.IsInRole("Admin");

                if (isAdmin && departmentIdFromAdmin.HasValue)
                {
                    // User is an admin, is a manager, and provided departmentIdFromAdmin
                    var serviceResponse = await _budgetTypeService.GetBudgetInformationByDepartmentId(departmentIdFromAdmin.Value);
                    return HandleResponse(serviceResponse);
                }
                else
                {
                    // Extract the department ID claim from the authenticated user's claims
                    var departmentClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimaryGroupSid);

                    if (departmentClaim != null && int.TryParse(departmentClaim.Value, out int departmentId))
                    {
                        var serviceResponse = await _budgetTypeService.GetBudgetInformationByDepartmentId(departmentId);
                        return HandleResponse(serviceResponse);
                    }
                    else
                    {
                        return BadRequest("Invalid or missing department ID claim");
                    }
                }
            }
            else
            {
                return Unauthorized("User not authenticated");
            }
        }


        [HttpPost("createBudgetType")]
        public async Task<IActionResult> CreateBudgetType(BudgetTypeDto budgetTypeDto)
        {
            var serviceResponse = await _budgetTypeService.CreateBudgetType(budgetTypeDto);
            return HandleResponse(serviceResponse);
        }

        [HttpPut("updateBudgetType")]
        public async Task<IActionResult> UpdateBudgetType(BudgetTypeDto budgetTypeDto)
        {
            var serviceResponse = await _budgetTypeService.UpdateBudgetType(budgetTypeDto);
            return HandleResponse(serviceResponse);
        }


    }
}
