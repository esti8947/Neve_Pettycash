using Microsoft.EntityFrameworkCore;
using DAL.Models;
using PettyCashNeve_ServerSide.Exceptions;
using DAL.Data;

namespace PettyCashNeve_ServerSide.Repositories.DepartmentRepository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly PettyCashNeveDbContext _context;
        public DepartmentRepository(PettyCashNeveDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            try
            {
                IQueryable<Department> departmentsQuery =_context.Departments.Where(d => d.IsCurrent == true);
                var departmentsDB = await departmentsQuery.ToListAsync();
                return departmentsDB;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<Department> GetDepartmentByIdAsync(int departmentId)
        {
            try
            {
                var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == departmentId && d.IsCurrent == true);
                if (department == null)
                {
                    throw new NotFoundException("No department found");
                }
                return department;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            try
            {
                var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == departmentId && d.IsCurrent == true);
                if (department == null)
                {
                    throw new NotFoundException("Department not found or not active");
                }

                department.IsCurrent = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task UpdateDepartmentAsync(Department updatedDepartment)
        {
            try
            {
                var existingDepartment = await _context.Departments.FindAsync(updatedDepartment.DepartmentId);

                if (existingDepartment == null || !existingDepartment.IsCurrent == true)
                {
                    throw new NotFoundException("Department not found or not active");
                }

                existingDepartment.DepartmentName = updatedDepartment.DepartmentName;
                existingDepartment.DepartmentCode = updatedDepartment.DepartmentCode;
                existingDepartment.DeptHeadFirstName = updatedDepartment.DeptHeadFirstName;
                existingDepartment.DeptHeadLastName = updatedDepartment.DeptHeadLastName;
                existingDepartment.Description = updatedDepartment.Description;
                existingDepartment.CurrentBudgetTypeId = updatedDepartment.CurrentBudgetTypeId;
                existingDepartment.PhonePrefix = updatedDepartment.PhonePrefix;
                existingDepartment.PhoneNumber = updatedDepartment.PhoneNumber;


                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<bool> CreateDepartment(Department department)
        {
            try
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
