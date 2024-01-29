using DAL.Models;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using PettyCashNeve_ServerSide.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.MonthlyBudgetRepository
{
    public class MonthlyBudgetRepository: IMonthlyBudgetRepository
    {
        private readonly PettyCashNeveDbContext _context;
        public MonthlyBudgetRepository(PettyCashNeveDbContext context)
        {
            _context = context;
        }
        public async Task<MonthlyBudget> CreateMonthlyBudgetAsync(MonthlyBudget monthlyBudget)
        {

            try
            {
                _context.MonthlyBudgets.Add(monthlyBudget);
                await _context.SaveChangesAsync();
                return monthlyBudget;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<List<MonthlyBudget>> GetMonthlyBudgetsByDepartmentIdAsync(int departmentId)
        {
            try
            {
                var monthlyBudgets = await _context.MonthlyBudgets
                    .Where(ab => ab.DepartmentId == departmentId)
                    .ToListAsync();

                return monthlyBudgets;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<MonthlyBudget> GetMonthlyBudgetsByDepartmentIdAndIsActiveAsync(int departmentId)
        {
            try
            {
                var monthlyBudget = await _context.MonthlyBudgets
                    .FirstOrDefaultAsync(ab => ab.DepartmentId == departmentId && ab.IsActive == true);
                if (monthlyBudget == null)
                {
                    throw new NotFoundException("Annual budget not found or not active");
                }
                return monthlyBudget;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<bool> AddSumToMonthlyBudgetCeilingAsync(int monthlyBudgetId, int additionalAmount)
        {
            try
            {
                var monthlyBudget = await _context.MonthlyBudgets.FindAsync(monthlyBudgetId);

                if (monthlyBudget == null || !monthlyBudget.IsActive == true)
                {
                    throw new NotFoundException("Annual budget not found or not active");
                }

                monthlyBudget.MonthlyBudgetCeiling += additionalAmount;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<bool> DeleteMonthlyBudgetAsync(int monthlyBudgetId)
        {
            try
            {
                var monthlyBudget = await _context.MonthlyBudgets.FindAsync(monthlyBudgetId);

                if (monthlyBudget == null || !monthlyBudget.IsActive == true)
                {
                    throw new NotFoundException("Annual budget not found or not active");
                }

                monthlyBudget.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

    }
}
