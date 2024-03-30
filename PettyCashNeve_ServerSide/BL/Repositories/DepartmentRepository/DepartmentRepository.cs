using Microsoft.EntityFrameworkCore;
using DAL.Models;
using PettyCashNeve_ServerSide.Exceptions;
using DAL.Data;
using BL.Repositories.AnnualBudgetRepository;
using BL.Repositories.MonthlyBudgetRepository;
using BL.Repositories.RefundBudgetRepository;
using BL.Repositories.ExpenseRepository;
using BL.Repositories.EventRepository;
using PettyCashNeve_ServerSide.Repositories.MonthlyCashRegisterRepository;

namespace PettyCashNeve_ServerSide.Repositories.DepartmentRepository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly PettyCashNeveDbContext _context;
        private readonly IAnnualBudgetRepository _annualBudgetRepository;
        private readonly IMonthlyBudgetRepository _monthlyBudgetRepository;
        private readonly IRefundBudgetRepository _refundBudgetRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMonthlyCashRegisterRepository _monthlyCashRegisterRepository;
        public DepartmentRepository(PettyCashNeveDbContext context, IAnnualBudgetRepository annualBudgetRepository, IMonthlyBudgetRepository monthlyBudgetRepository, IRefundBudgetRepository refundBudgetRepository
            , IExpenseRepository expenseRepository, IEventRepository eventRepository, IMonthlyCashRegisterRepository monthlyCashRegisterRepository)
        {
            _context = context;
            _annualBudgetRepository = annualBudgetRepository;
            _monthlyBudgetRepository = monthlyBudgetRepository;
            _refundBudgetRepository = refundBudgetRepository;
            _expenseRepository = expenseRepository;
            _eventRepository = eventRepository;
            _monthlyCashRegisterRepository = monthlyCashRegisterRepository;
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
        public async Task<List<Department>> GetInactiveDepartments()
        {
            try
            {
                IQueryable<Department> departmentsQuery = _context.Departments.Where(d => d.IsCurrent == false);
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

        public async Task<bool> ActivateDepartment(int departmentId)
        {
            try
            {
                var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == departmentId && d.IsCurrent == false);
                if (department == null)
                {
                    throw new NotFoundException("Department not found or not active");
                }

                department.IsCurrent = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<bool> UpdateDepartmentAsync(Department updatedDepartment)
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
                return true;

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
                    u => u.DepartmentId == departmentId && u.IsActive).ToListAsync();
                return usersList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<int> GetBudgetTypeIdByDepartmentId(int departmentId)
        {
            var department = await GetDepartmentByIdAsync(departmentId);
            int budgetTypeId = department.CurrentBudgetTypeId;
            return budgetTypeId;
        }

        public async Task<int> GetYearByDepartmentId(int departmentId)
        {
            int year = 0;
            try
            {
                var budgetTypeId = await GetBudgetTypeIdByDepartmentId(departmentId);
                if (budgetTypeId != null || budgetTypeId != 0)
                {
                    switch (budgetTypeId)
                    {
                        case 1:
                            var annualBudgetResponse = await _annualBudgetRepository.GetAnnualBudgetsByDepartmentIdAndIsActiveAsync(departmentId);
                            if (annualBudgetResponse != null)
                            {
                                year = annualBudgetResponse.AnnualBudgetYear;
                            }
                            break;

                        case 2:
                            var monthlyBudgetResponse = await _monthlyBudgetRepository.GetMonthlyBudgetsByDepartmentIdAndIsActiveAsync(departmentId);
                            if (monthlyBudgetResponse != null)
                            {
                                year = monthlyBudgetResponse.MonthlyBudgetYear;
                            }
                            break;
                        case 3:
                            var refundBudgetResponse = await _refundBudgetRepository.GetRefundBudgetByDepartmentIdAndIsActiveAsync(departmentId);
                            if (refundBudgetResponse != null)
                            {
                                year = refundBudgetResponse.RefundBudgetYear;
                            }
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

        public async Task<bool> deactivateBudget(int departmentId)
        {
            bool issucceed = false;
            var budgetTypeId = await GetBudgetTypeIdByDepartmentId(departmentId);
            if (budgetTypeId != null || budgetTypeId != 0)
            {
                switch (budgetTypeId)
                {
                    case 1:
                        var annualBudgetResponse = await _annualBudgetRepository.GetAnnualBudgetsByDepartmentIdAndIsActiveAsync(departmentId);
                        if (annualBudgetResponse != null)
                        {
                            issucceed = await _annualBudgetRepository.deactivateAnnualBudget(departmentId);
                        }
                        break;

                    case 2:
                        var monthlyBudgetResponse = await _monthlyBudgetRepository.GetMonthlyBudgetsByDepartmentIdAndIsActiveAsync(departmentId);
                        if (monthlyBudgetResponse != null)
                        {
                            issucceed = await _monthlyBudgetRepository.deactivateMonthlyBudget(departmentId);
                        }
                        break;
                    case 3:
                        var refundBudgetResponse = await _refundBudgetRepository.GetRefundBudgetByDepartmentIdAndIsActiveAsync(departmentId);
                        if (refundBudgetResponse != null)
                        {
                            issucceed = await _refundBudgetRepository.deactivateRefundBudget(departmentId);
                        }
                        break;
                }
            }
            return issucceed;
        }

        public async Task<bool> DeleteDepartmentAndAssociatedDataAsync(int departmentId)
        {
            try
            {
                var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == departmentId);


                if (department == null)
                {
                    throw new NotFoundException("Department not found or not active");
                }

                var expenses = await _expenseRepository.GetExpensesOfDepartment(departmentId);
                if (expenses != null && expenses.Any())
                {
                    _context.Expenses.RemoveRange(expenses);
                }

                var events = await _eventRepository.GetEventsByDepartmentId(departmentId);
                if (events != null && events.Any())
                {
                    _context.Events.RemoveRange(events);
                }

                var monthlyCashRegisters = await _monthlyCashRegisterRepository.GetAllMonthlyCashRegisterByDepartmenId(departmentId);
                if (monthlyCashRegisters != null && monthlyCashRegisters.Any())
                {
                    _context.MonthlyCashRegisters.RemoveRange(monthlyCashRegisters);
                }

                var annualBudgets = await _annualBudgetRepository.GetAnnualBudgetsByDepartmentIdAsync(departmentId);
                if (annualBudgets != null && annualBudgets.Any())
                {
                    _context.AnnualBudgets.RemoveRange(annualBudgets);
                }

                var monthlyBudgets = await _monthlyBudgetRepository.GetMonthlyBudgetsByDepartmentIdAsync(departmentId);
                if (monthlyBudgets != null && monthlyBudgets.Any())
                {
                    _context.MonthlyBudgets.RemoveRange(monthlyBudgets);
                }

                var refundBudgets = await _refundBudgetRepository.GetRefundBudgetsByDepartmentIdAsync(departmentId);
                if (refundBudgets != null && refundBudgets.Any())
                {
                    _context.RefundBudgets.RemoveRange(refundBudgets);
                }

                var users = await GetUsersByDepartmentId(departmentId);
                if (users != null && users.Any())
                {
                    _context.Users.RemoveRange(users);
                }

                _context.Departments.Remove(department);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
