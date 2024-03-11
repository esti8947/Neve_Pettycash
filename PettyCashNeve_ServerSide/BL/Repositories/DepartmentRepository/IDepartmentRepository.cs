using DAL.Models;

namespace PettyCashNeve_ServerSide.Repositories.DepartmentRepository
{
    public interface IDepartmentRepository
    {
        public Task<List<Department>> GetDepartmentsAsync();
        public Task<Department> GetDepartmentByIdAsync(int departmentId);
        public Task<bool> UpdateDepartmentAsync(Department updatedDepartment);
        public Task<bool> DeleteDepartmentAsync(int departmentID);
        public Task<bool> CreateDepartment(Department department);
        public Task<List<NdbUser>> GetUsersByDepartmentId(int departmentId);
        public Task<bool> deactivateBudget(int departmentId);
        public Task<int> GetYearByDepartmentId(int departmentId);
    }
}
