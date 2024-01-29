using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PettyCashNeve_ServerSide.Dto;
using PettyCashNeve_ServerSide.Services.MonthlyCashRegisterService;
using System.Security.Claims;

namespace PettyCashNeve_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MonthlyCashRegisterController : BaseController
    {
        private readonly IMonthlyCashRegisterService _monthlyCashRegisterService;


        public MonthlyCashRegisterController(IMonthlyCashRegisterService monthlyCashRegisterService)
        {
            _monthlyCashRegisterService = monthlyCashRegisterService;
        }


        [HttpGet("getMonthlyCashRegistersByUserId")]
        public async Task<IActionResult> GetMonthlyCashRegistersByUserId([FromQuery] int? departmentId = null)
        {
            try
            {
                var userId = UserId;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserId is null or empty.");
                }

                // Check if the current user is a manager
                bool isManager = User.IsInRole("Admin");

                if (isManager && departmentId.HasValue)
                {
                    // Manager can specify a departmentId
                    var serviceResponse = await _monthlyCashRegisterService.GetCurrentMonthlyCashRegistersByDepartmentIdAsync(departmentId.Value);
                    return HandleResponse(serviceResponse);
                }
                else
                {
                    // Use the userId for non-manager users or when no departmentId is specified
                    var serviceResponse = await _monthlyCashRegisterService.GetCurrentMonthlyCashRegistersByUserIdAsync(userId);
                    return HandleResponse(serviceResponse);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet("getMonthlyCashRegistersByUserOfDepartmentId/{userOfDepartmentId}")]
        //public async Task<IActionResult> GetMonthlyCashRegistersByUserOfDepartmentId(int userOfDepartmentId)
        //{
        //    var serviceResponse = await _monthlyCashRegisterService.GetMonthlyCashRegistersByUserOfDepartmentIdAsync(userOfDepartmentId);
        //    return HandleResponse(serviceResponse);
        //}

        //[HttpGet("getMonthlyCashRegistersByDepartmentId/{departmentId}")]
        //public async Task<IActionResult> GetMonthlyCashRegistersByDepartmentId(int departmentId)
        //{
        //    var serviceResponse = await _monthlyCashRegisterService.GetMonthlyCashRegistersByDepartmentIdAsync(departmentId);
        //    return HandleResponse(serviceResponse);
        //}

        [HttpPost("createNewMonthlyCashRegister")]
        public async Task<IActionResult> CreateNewMonthlyCashRegister([FromBody] MonthlyCashRegisterDto newMonthlyCashRegister)
        {
            try
            {
                var userId = UserId;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserId is null or empty.");
                }

                newMonthlyCashRegister.UpdatedBy = userId;

                    var serviceResponse = await _monthlyCashRegisterService.CreateNewMonthlyCashRegisterAsync(newMonthlyCashRegister);
                    return HandleResponse(serviceResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("updateMonthlyCashRegister")]
        public async Task<IActionResult> UpdateMonthlyCashRegister([FromBody] MonthlyCashRegisterDto updateMonthlyCashRegisterDto)
        {
            var serviceResponse = await _monthlyCashRegisterService.UpdateMonthlyCashRegisterAsync(updateMonthlyCashRegisterDto);
            return HandleResponse(serviceResponse);
        }

        [HttpGet("deactivateMonthlyCashRegister")]
        public async Task<IActionResult> DeactivateMonthlyCashRegister()
        {
            var userId = UserId;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is null or empty.");
            }
            var serviceResponse = await _monthlyCashRegisterService.DeactivateMonthlyCashRegister(userId);
            return HandleResponse(serviceResponse);
        }

        [HttpGet("insertRefundAmount/{refundAmount}")]
        public async Task<IActionResult> InsertRefundAmount(decimal refundAmount)
        {
            var userId = UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is null or empty.");
            }
            var serviceResponse = await _monthlyCashRegisterService.InsertRefundAmount(refundAmount, userId);
            return HandleResponse(serviceResponse);
        }

    }
}
