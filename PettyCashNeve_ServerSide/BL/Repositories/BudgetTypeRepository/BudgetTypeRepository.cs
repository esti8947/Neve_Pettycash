using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.BudgetTypeRepository
{
    public class BudgetTypeRepository : IBudgetTypeRepository
    {
        private readonly PettyCashNeveDbContext _context;
        public BudgetTypeRepository(PettyCashNeveDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateBudgetTypeAsync(BudgetType newBudgetType)
        {
            try
            {
                _context.BudgetTypes.Add(newBudgetType);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<BudgetType> GetBudgetTypeByIdAsync(int id)
        {
            try
            {
                var budgetType = await _context.BudgetTypes.FirstOrDefaultAsync(b => b.BudgetTypeId == id);
                return budgetType;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<List<BudgetType>> GetBudgetTypesAsync()
        {
            try
            {
                return _context.BudgetTypes.ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> UpdateBudgetTypeAsync(BudgetType updatedBudgetType)
        {
            try
            {
                var existingBudgetType = await _context.BudgetTypes.FirstOrDefaultAsync(b => b.BudgetTypeId == updatedBudgetType.BudgetTypeId);
                if (existingBudgetType != null)
                {
                    existingBudgetType.BudgetTypeName = updatedBudgetType.BudgetTypeName;
                    existingBudgetType.BudgetTypeDescription = updatedBudgetType.BudgetTypeDescription;
                    existingBudgetType.budgetTypeNameHeb = updatedBudgetType.budgetTypeNameHeb;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
