﻿using Entities.Models_Dto;
using BL.Services;
using PettyCashNeve_ServerSide.Dto;
using Entities.Models_Dto.UserDto;
using DAL.Models;

namespace PettyCashNeve_ServerSide.Services.DepartmentService
{
    public interface IDepartmentService
    {
        Task<ServiceResponse<List<DepartmentDto>>> GetDepartmentsAsync();
        public Task<ServiceResponse<List<DepartmentDto>>> GetInactiveDepartments();
        Task<ServiceResponse<DepartmentDto>> GetDepartmentByIdAsync(int departmentID);
        Task<ServiceResponse<bool>> UpdateDepartmentAsync(DepartmentDto updatedDepartment);
        Task<ServiceResponse<bool>> DeleteDepartmentAsync(int departmentId);
        public Task<ServiceResponse<bool>> ActivateDepartment(int departmentId);
        Task<ServiceResponse<bool>> DeleteDepartmentAndAssociatedDataAsync(int departmentId);
        Task<ServiceResponse<bool>> CreateDepartmentAsync(DepartmentDto departmentDto);
        Task<ServiceResponse<List<UserInfoModel>>> GetUsersByDepartmentId(int departmentId);
        Task<ServiceResponse<bool>> deactivateBudget(int departmentId);
        Task<ServiceResponse<int>> GetYearByDepartmentId(int departmentId);
        
    }
}
