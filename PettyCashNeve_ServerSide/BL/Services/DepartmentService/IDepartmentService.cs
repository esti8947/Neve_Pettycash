using Entities.Models_Dto;
using BL.Services;
using PettyCashNeve_ServerSide.Dto;
using Entities.Models_Dto.UserDto;

namespace PettyCashNeve_ServerSide.Services.DepartmentService
{
    public interface IDepartmentService
    {
        Task<ServiceResponse<List<DepartmentDto>>> GetDepartmentsAsync();
        Task<ServiceResponse<DepartmentDto>> GetDepartmentByIdAsync(int departmentID);
        Task<ServiceResponse<bool>> UpdateDepartmentAsync(DepartmentDto updatedDepartment);
        Task<ServiceResponse<bool>> DeleteDepartmentAsync(int departmentId);
        Task<ServiceResponse<bool>> CreateDepartmentAsync(DepartmentDto departmentDto);
        Task<ServiceResponse<List<UserDto>>> GetUsersByDepartmentId(int departmentId);
        Task<ServiceResponse<int>> GetYearByDepartmentId(int departmentId);
        
    }
}
