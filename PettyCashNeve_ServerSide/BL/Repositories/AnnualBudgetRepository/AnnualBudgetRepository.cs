using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using PettyCashNeve_ServerSide.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.AnnualBudgetRepository
{
    public class AnnualBudgetRepository : IAnnualBudgetRepository
    {
        private readonly PettyCashNeveDbContext _context;
        public AnnualBudgetRepository(PettyCashNeveDbContext context)
        {
            _context = context;
        }
        public async Task<AnnualBudget> CreateAnnualBudgetAsync(AnnualBudget annualBudget)
        {

            try
            {
                _context.AnnualBudgets.Add(annualBudget);
                await _context.SaveChangesAsync();
                return annualBudget;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<List<AnnualBudget>> GetAnnualBudgetsByDepartmentIdAsync(int departmentId)
        {
            try
            {
                var annualBudgets = await _context.AnnualBudgets
                    .Where(ab => ab.DepartmentId == departmentId)
                    .ToListAsync();

                return annualBudgets;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<AnnualBudget> GetAnnualBudgetsByDepartmentIdAndIsActiveAsync(int departmentId)
        {
            try
            {
                var annualBudgets = await _context.AnnualBudgets
                    .FirstOrDefaultAsync(ab => ab.DepartmentId == departmentId && ab.IsActive == true);
                if(annualBudgets == null)
                {
                    throw new NotFoundException("Annual budget not found or not active");
                }
                return annualBudgets;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<bool> AddSumToAnnualBudgetCeilingAsync(int annualBudgetId, int additionalAmount)
        {
            try
            {
                var annualBudget = await _context.AnnualBudgets.FindAsync(annualBudgetId);

                if (annualBudget == null || !annualBudget.IsActive == true)
                {
                    throw new NotFoundException("Annual budget not found or not active");
                }

                annualBudget.AnnualBudgetCeiling += additionalAmount;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<bool> DeleteAnnualBudgetAsync(int annualBudgetId)
        {
            try
            {
                var annualBudget = await _context.AnnualBudgets.FindAsync(annualBudgetId);

                if (annualBudget == null || !annualBudget.IsActive == true)
                {
                    throw new NotFoundException("Annual budget not found or not active");
                }

                annualBudget.IsActive = false;
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
