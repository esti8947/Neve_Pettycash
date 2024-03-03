using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.ExpenseRepository
{
    public interface IExpenseRepository
    {
        Task<List<Expenses>> GetAllExpensesAsync();
        Task<List<Expenses>> GetExpensesOfUserAsync(string updatedBy);
        Task<List<Expenses>> GetUnapprovedExpensesByUserAsync(string updatedBy);
        Task<List<Expenses>> GetApprovedAndUnlockedExpensesOfDepartment(int departmentId);
        Task<List<Expenses>> GetExpensesOfUserByYear(string updatedBy, int year);
        Task<List<Expenses>> GetExpensesOfDepartmentByYear(int departmentId, int year);
        Task<bool> UpdateExpenseAsync(Expenses updatedExpense);
        Task<bool> DeleteExpenseAsync(int expenseId);
        Task<bool> CreateExpenseAsync(Expenses newExpense);
        Task<bool> ApproveExpenses(List<Expenses> expenses);
        Task<bool> ApproveAllExpenses(string userId, int year, int month);
        Task<bool> LockExpenses(int month, int year,int departmentId);
        //Task<decimal> GetTotalExpenseAmountForMonthYearAsync(int month, int year, string userId);
        Task<List<Expenses>> GetExpensesOfDepartment(int departmentId);
        Task<List<Expenses>> GetUnlockedExpensesOfDepartment(int departmentId);
        Task<List<Expenses>> GetActiveExpensesByUserIdAndDate(int month, int year, string updateBy);
        Task<List<Expenses>> GetActiveExpensesByDepartmentIdAndDate(int month, int year, int departmentId);
        Task<decimal> GetTotalAmountByYearRangeAndDepartmentAsync(int yearRange, int departmentId);
        Task<List<Expenses>> GetExpensesByYearRangeAndDepartmentAsync(int yearRange, int departmentId);
        Task<decimal> GetExpensesAmountForMonth(int month, int year, string userId);
        Task<decimal> GetExpensesAmountForMonthByDepartmentId(int month, int year, int departmentId);
        Task<bool> IsExpensesLockedAndApprovedForDepartmentAsync(int departmentId, int yearRange);


    }
}
