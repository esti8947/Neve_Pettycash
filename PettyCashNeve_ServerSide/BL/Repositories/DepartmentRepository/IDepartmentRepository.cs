using DAL.Models;

namespace PettyCashNeve_ServerSide.Repositories.DepartmentRepository
{
    public interface IDepartmentRepository
    {
        public Task<List<Department>> GetDepartmentsAsync();
        public Task<Department> GetDepartmentByIdAsync(int departmentId);
        public Task UpdateDepartmentAsync(Department updatedDepartment);
        public Task<bool> DeleteDepartmentAsync(int departmentID);
        public Task<bool> CreateDepartment(Department department);
        public Task<List<NdbUser>> GetUsersByDepartmentId(int departmentIk);
    }
}
