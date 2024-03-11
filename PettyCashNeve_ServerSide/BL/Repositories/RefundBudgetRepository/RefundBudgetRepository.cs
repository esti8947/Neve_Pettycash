using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using PettyCashNeve_ServerSide.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.RefundBudgetRepository
{
    public class RefundBudgetRepository : IRefundBudgetRepository
    {
        private readonly PettyCashNeveDbContext _context;
        public RefundBudgetRepository(PettyCashNeveDbContext context)
        {
            _context = context;
        }
        public async Task<RefundBudget> CreateRefundBudgetAsync(RefundBudget refundBudget)
        {

            try
            {
                _context.RefundBudgets.Add(refundBudget);
                await _context.SaveChangesAsync();
                return refundBudget;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<List<RefundBudget>> GetRefundBudgetsByDepartmentIdAsync(int departmentId)
        {
            try
            {
                var refundBudgets = await _context.RefundBudgets
                    .Where(ab => ab.DepartmentId == departmentId)
                    .ToListAsync();

                return refundBudgets;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<RefundBudget> GetRefundBudgetByDepartmentIdAndIsActiveAsync(int departmentId)
        {
            try
            {
                var refundBudget = await _context.RefundBudgets
                    .FirstOrDefaultAsync(ab => ab.DepartmentId == departmentId && ab.IsActive == true);
                if (refundBudget == null)
                {
                    return null;
                }
                return refundBudget;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<bool> deactivateRefundBudget(int departmentId)
        {
            try
            {
                var refundBudget = await GetRefundBudgetByDepartmentIdAndIsActiveAsync(departmentId);
                if(refundBudget != null)
                {
                    refundBudget.IsActive = false;
                    await _context.SaveChangesAsync();
                }
                return true;

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<bool> DeleteRefundBudgetAsync(int refundBudgetId)
        {
            try
            {
                var refundBudget = await _context.RefundBudgets.FindAsync(refundBudgetId);

                if (refundBudget == null || !refundBudget.IsActive == true)
                {
                    throw new NotFoundException("Annual budget not found or not active");
                }

                refundBudget.IsActive = false;
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
