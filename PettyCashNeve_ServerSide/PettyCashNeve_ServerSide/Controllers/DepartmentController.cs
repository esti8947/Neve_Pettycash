﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PettyCashNeve_ServerSide.Dto;
using PettyCashNeve_ServerSide.Services.DepartmentService;

namespace PettyCashNeve_ServerSide.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
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

        [HttpPut("updateDepartment")]
        public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentDto updatedDepartment)
        {
            var serviceResponse = await _departmentService.UpdateDepartmentAsync(updatedDepartment);
            return HandleResponse(serviceResponse);
        }

        [HttpPost("createDepartment")]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto newDepartment)
        {
            var serviceResponse = await _departmentService.CreateDepartmentAsync(newDepartment);
            return HandleResponse(serviceResponse);
        }
    }
}
