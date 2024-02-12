using Microsoft.EntityFrameworkCore;
using DAL.Models;
using PettyCashNeve_ServerSide.Exceptions;
using DAL.Data;
using BL.Repositories.AnnualBudgetRepository;
using BL.Repositories.MonthlyBudgetRepository;
using BL.Repositories.RefundBudgetRepository;

namespace PettyCashNeve_ServerSide.Repositories.DepartmentRepository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly PettyCashNeveDbContext _context;
        private readonly IAnnualBudgetRepository _annualBudgetRepository;
        private readonly IMonthlyBudgetRepository _monthlyBudgetRepository;
        private readonly IRefundBudgetRepository _refundBudgetRepository;
        public DepartmentRepository(PettyCashNeveDbContext context, IAnnualBudgetRepository annualBudgetRepository, IMonthlyBudgetRepository monthlyBudgetRepository, IRefundBudgetRepository refundBudgetRepository)
        {
            _context = context;
            _annualBudgetRepository = annualBudgetRepository;
            _monthlyBudgetRepository = monthlyBudgetRepository;
            _refundBudgetRepository = refundBudgetRepository;
        }

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            try
            {
                IQueryable<Department> departmentsQuery = _context.Departments.Where(d => d.IsCurrent == true);
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

        public async Task<List<NdbUser>> GetUsersByDepartmentId(int departmentId)
        {
            try
            {
                var usersList = await _context.Users.Where(
                    u => u.DepartmentId == departmentId).ToListAsync();
                return usersList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<int> GetYearByDepartmentId(int departmentId)
        {
            int year = 0;
            try
            {
                var department = GetDepartmentByIdAsync(departmentId);
                var budgetTypeId = department.Result.CurrentBudgetTypeId;
                if (budgetTypeId != null || budgetTypeId != 0)
                {
                    switch (budgetTypeId)
                    {
                        case 1:
                            var annualBudgetResponse = await _annualBudgetRepository.GetAnnualBudgetsByDepartmentIdAndIsActiveAsync(departmentId);
                            year = annualBudgetResponse.AnnualBudgetYear;
                            annualBudgetResponse.IsActive = false;
                            await _context.SaveChangesAsync();
                            break;

                        case 2:
                            var monthlyBudgetResponse = await _monthlyBudgetRepository.GetMonthlyBudgetsByDepartmentIdAndIsActiveAsync(departmentId);
                            year = monthlyBudgetResponse.MonthlyBudgetYear;
                            monthlyBudgetResponse.IsActive = false;
                            await _context.SaveChangesAsync();
                            break;
                        case 3:
                            var refundBudgetResponse = await _refundBudgetRepository.GetRefundBudgetByDepartmentIdAndIsActiveAsync(departmentId);
                            year = refundBudgetResponse.RefundBudgetYear;
                            refundBudgetResponse.IsActive = false;
                            await _context.SaveChangesAsync();
                        break;
                    }
                }
                return year;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
