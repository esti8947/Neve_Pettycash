using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PettyCashNeve_ServerSide.Dto;
using PettyCashNeve_ServerSide.Services.DepartmentService;

namespace PettyCashNeve_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("getDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            var serviceResponse = await _departmentService.GetDepartmentsAsync();
            return HandleResponse(serviceResponse);
        }
        [HttpGet("getInactiveDepartments")]
        public async Task<IActionResult> GetInactiveDepartments()
        {
            var serviceResponse = await _departmentService.GetInactiveDepartments();
            return HandleResponse(serviceResponse);
        }

        [HttpGet("getDepartmentById/{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var serviceResponse = await _departmentService.GetDepartmentByIdAsync(id);
            return HandleResponse(serviceResponse);
        }

        [HttpDelete("deleteDepartment/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var serviceResponse = await _departmentService.DeleteDepartmentAsync(id);
            return HandleResponse(serviceResponse);
        }

        [HttpGet("activateDepartment/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateDepartment(int id)
        {
            var serviceResponse = await _departmentService.ActivateDepartment(id);
            return HandleResponse(serviceResponse);
        }

        [HttpDelete("deleteDepartmentAndAssociatedData/{departmentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartmentAndAssociatedData(int departmentId)
        {
            var serviceResponse = await _departmentService.DeleteDepartmentAndAssociatedDataAsync(departmentId);
            return HandleResponse(serviceResponse);
        }

        [HttpPut("updateDepartment")]
        public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentDto updatedDepartment)
        {
            var serviceResponse = await _departmentService.UpdateDepartmentAsync(updatedDepartment);
            return HandleResponse(serviceResponse);
        }

        [HttpPost("createDepartment")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto newDepartment)
        {
            var serviceResponse = await _departmentService.CreateDepartmentAsync(newDepartment);
            return HandleResponse(serviceResponse);
        }

        [HttpGet("getUsersByDepartmentId/{departmentId}")]
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> GetUsersByDepartmentId(int departmentId)
        {
            var serviceResponse = await _departmentService.GetUsersByDepartmentId(departmentId);
            return HandleResponse(serviceResponse);
        }
    }
}
