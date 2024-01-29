using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.ExpenseCategoryRepository
{
    public interface IExpenseCategoryRepository
    {
        Task<List<ExpenseCategory>> GetAllExpenseCategoryAsync();
        Task<ExpenseCategory> GetExpenseCategoryByIdAsync(int id);
        Task<string> GetExpenseCategoryNameByIdAsync(int id);
        Task<bool> CreateExpenseCategoryAsync(ExpenseCategory expenseCategory);
        Task<bool> DeleteExpenseCategory(int expenseCategoryId);
    }
}
