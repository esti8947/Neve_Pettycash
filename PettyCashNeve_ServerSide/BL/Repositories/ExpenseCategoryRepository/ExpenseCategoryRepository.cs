using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using PettyCashNeve_ServerSide.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.ExpenseCategoryRepository
{
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        private readonly PettyCashNeveDbContext _context;
        public ExpenseCategoryRepository(PettyCashNeveDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateExpenseCategoryAsync(ExpenseCategory expenseCategory)
        {
            try
            {
                _context.ExpenseCategories.Add(expenseCategory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<ExpenseCategory>> GetAllExpenseCategoryAsync()
        {
            var expensesCategory = await _context.ExpenseCategories.Where(ec => ec.IsActive).ToListAsync();
            return expensesCategory;
        }

        public async Task<ExpenseCategory> GetExpenseCategoryByIdAsync(int id)
        {
            try
            {
                var expenseCategory = await _context.ExpenseCategories
                     .FirstOrDefaultAsync(ec => ec.ExpenseCategoryId == id);
                return expenseCategory;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<string> GetExpenseCategoryNameByIdAsync(int id)
        {
            try
            {
                var expenseCategoryName = await _context.ExpenseCategories
                    .Where(ec => ec.ExpenseCategoryId == id)
                    .Select(ec => ec.ExpenseCategoryName)
                    .FirstOrDefaultAsync();

                return expenseCategoryName;
            }
            catch (Exception ex)
            {
                // Handle exception (log, throw, etc.)
                throw;
            }
        }

        public async Task<bool> DeleteExpenseCategory(int expenseCategoryId)
        {
            var expenseCategoryToDelete = await _context.ExpenseCategories
                .FirstOrDefaultAsync(ec => ec.ExpenseCategoryId == expenseCategoryId);
            if(expenseCategoryToDelete != null)
            {
                try
                {
                    expenseCategoryToDelete.IsActive = false;
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return false;

        }

        public async Task<bool> UpdateExpenseCategory(ExpenseCategory updatedExpneseCategory)
        {
            try
            {
                var existingExpenseCategory = await _context.ExpenseCategories.FindAsync(updatedExpneseCategory.ExpenseCategoryId);

                if (existingExpenseCategory == null || !existingExpenseCategory.IsActive == true)
                {
                    throw new NotFoundException("Expense category not found or not active");
                }
                existingExpenseCategory.ExpenseCategoryName = updatedExpneseCategory.ExpenseCategoryName;
                existingExpenseCategory.ExpenseCategoryNameHeb = updatedExpneseCategory.ExpenseCategoryNameHeb;
                existingExpenseCategory.expenseCategoryType = updatedExpneseCategory.expenseCategoryType;
                existingExpenseCategory.AccountingCode = updatedExpneseCategory.AccountingCode;
               

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
