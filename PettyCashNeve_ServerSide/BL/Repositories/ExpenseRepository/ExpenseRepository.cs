using DAL.Models;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using PettyCashNeve_ServerSide.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace BL.Repositories.ExpenseRepository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly PettyCashNeveDbContext _context;
        private readonly UserManager<NdbUser> _userManager;
        public ExpenseRepository(PettyCashNeveDbContext context, UserManager<NdbUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Expenses>> GetAllExpensesAsync()
        {
            var expenses = await _context.Expenses.Where(e => e.IsActive == true)
                .ToListAsync();
            return expenses;
        }

        public async Task<List<Expenses>> GetExpensesOfUserAsync(string updatedBy)
        {
            try
            {
                var expensesList = await _context.Expenses.FromSqlRaw("EXEC GetActiveExpensesByUserId @UserId", new SqlParameter("@UserId", updatedBy))
                    .ToListAsync();
                return expensesList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<Expenses>> GetExpensesOfUserByYear(string updatedBy, int year)
        {
            try
            {
                var expensesList = await _context.Expenses
                  .FromSqlRaw("EXEC GetActiveExpensesByUserIdAndYear @UserId, @Year",
                                new SqlParameter("@UserId", updatedBy),
                                new SqlParameter("@Year", year))
                                .ToListAsync();
                return expensesList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<Expenses>> GetExpensesOfDepartmentByYear(int departmentId, int year)
        {
            try
            {
                var expensesList = await _context.Expenses
                  .FromSqlRaw("EXEC GetActiveExpensesByDepartmentIdAndYear @DepartmentId, @Year",
                                new SqlParameter("@DepartmentId", departmentId),
                                new SqlParameter("@Year", year))
                                .ToListAsync();
                return expensesList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<Expenses>> GetUnapprovedExpensesByUserAsync(string updatedBy)
        {
            try
            {
                var expenseList = await GetExpensesOfUserAsync(updatedBy);
                var unApprovedExpensesList = expenseList.Where(e => e.IsApproved == false)
                    .ToList();
                return unApprovedExpensesList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<Expenses>> GetApprovedAndUnlockedExpensesOfDepartment(int departmentId)
        {
            try
            {
                var expensesOfDepartment = await GetExpensesOfDepartment(departmentId);
                var approvedUnlockedExpenses = expensesOfDepartment.Where(
                        e => e.IsApproved && !e.IsLocked).ToList();

                var groupedExpenses = approvedUnlockedExpenses
                    .GroupBy(e => new { e.ExpenseDate.Year, e.ExpenseDate.Month })
                    .Select(group => group.ToList())
                    .ToList();

                // Check if all expenses within each group are approved and unlocked
                var filteredGroups = groupedExpenses
                    .Where(group => group.All(expense => expense.IsApproved && !expense.IsLocked));

                // Check if there are any unapproved or locked expenses for the same month and year in expensesOfDepartment
                var allApprovedUnlocked = expensesOfDepartment
                    .Where(e => e.IsApproved && !e.IsLocked)
                    .GroupBy(e => new { e.ExpenseDate.Year, e.ExpenseDate.Month })
                    .All(group => group.All(expense => expense.IsApproved && !expense.IsLocked));

                if (allApprovedUnlocked)
                {
                    // Flatten the filtered groups and return the resulting list of expenses
                    return filteredGroups.SelectMany(group => group).ToList();
                }
                else
                {
                    // If any expense within the department for the same month and year is not approved and unlocked, return an empty list
                    return new List<Expenses>();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<bool> UpdateExpenseAsync(Expenses updatedExpense)
        {
            try
            {
                var existingExpense = await _context.Expenses
                    .FirstOrDefaultAsync(e => e.ExpenseId == updatedExpense.ExpenseId && e.IsActive == true);
                if (existingExpense == null)
                {
                    throw new DirectoryNotFoundException("Expense not found or not active");
                }
                //_context.Entry(existingExpense).CurrentValues.SetValues(updatedExpense);
                existingExpense.ExpenseCategoryId = updatedExpense.ExpenseCategoryId;
                existingExpense.EventsId = updatedExpense.EventsId;
                existingExpense.UpdatedBy = updatedExpense.UpdatedBy;
                existingExpense.DepartmentId = updatedExpense.DepartmentId;
                existingExpense.StoreName = updatedExpense.StoreName;
                existingExpense.ExpenseDate = updatedExpense.ExpenseDate;
                existingExpense.RefundMonth = updatedExpense.RefundMonth;
                existingExpense.IsLocked = updatedExpense.IsLocked;
                existingExpense.IsApproved = updatedExpense.IsApproved;
                existingExpense.BuyerId = updatedExpense.BuyerId;
                existingExpense.InvoiceScan = updatedExpense.InvoiceScan;
                existingExpense.ExpenseAmount = updatedExpense.ExpenseAmount;
                existingExpense.Notes = updatedExpense.Notes;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> DeleteExpenseAsync(int expenseId)
        {
            try
            {
                var expense = await _context.Expenses
                    .FirstOrDefaultAsync(e => e.ExpenseId == expenseId && e.IsActive == true);

                if (expense == null)
                {
                    throw new NotFoundException("Expense not found or not active");
                }

                expense.IsActive = false;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<bool> CreateExpenseAsync(Expenses newExpense)
        {
            try
            {
                _context.Expenses.Add(newExpense);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> ApproveExpenses(List<Expenses> expenses)
        {
            try
            {
                foreach (var expense in expenses)
                {
                    var existingExpense = await _context.Expenses.FirstOrDefaultAsync(e => e.ExpenseId == expense.ExpenseId);

                    if (existingExpense != null)
                    {
                        existingExpense.IsApproved = true;
                    }
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ApproveAllExpenses(string userId, int year, int month)
        {
            try
            {
                var expensesList = await GetActiveExpensesByUserIdAndDate(month, year, userId);

                foreach (var expense in expensesList)
                {
                    expense.IsApproved = true;
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                return false;
            }
        }

        public async Task<List<Expenses>> GetActiveExpensesByUserIdAndDate(int month, int year, string userId)
        {
            try
            {
                var expensesList = await _context.Expenses
                   .FromSqlRaw("EXEC GetActiveExpensesByUserIdAndDate @Month, @Year, @UserId",
                    new SqlParameter("@Month", month),
                    new SqlParameter("@Year", year),
                    new SqlParameter("@UserId", userId))
                .ToListAsync();
                return expensesList;
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                throw;
            }
        }

        public async Task<List<Expenses>> GetActiveExpensesByDepartmentIdAndDate(int month, int year, int departmentId)
        {
            try
            {
                var expensesList = await _context.Expenses
                   .FromSqlRaw("EXEC GetActiveExpensesByDepartmentIdAndDate @Month, @Year, @DepartmentId",
                    new SqlParameter("@Month", month),
                    new SqlParameter("@Year", year),
                    new SqlParameter("@DepartmentId", departmentId))
                .ToListAsync();
                return expensesList;
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                throw;
            }
        }

        public async Task<List<Expenses>> GetExpensesOfDepartment(int departmentId)
        {
            try
            {
                var expensesList = await _context.Expenses.FromSqlRaw("EXEC GetActiveExpensesByDepartmentId @DepartmentId", new SqlParameter("@DepartmentId", departmentId))
                .ToListAsync();
                return expensesList;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<decimal> GetTotalAmountByYearRangeAndDepartmentAsync(int yearRange, int departmentId)
        {
            try
            {
                var startYear = yearRange / 10000;
                var endYear = yearRange % 10000;

                var parameters = new[]
                {
                    new SqlParameter("@YearRange", yearRange),
                    new SqlParameter("@DepartmentId", departmentId)
                };

                // Call the stored procedure and retrieve the total amount
                var totalAmount = await _context.Database
                    .ExecuteSqlRawAsync(
                        "EXEC GetTotalAmountByYearRangeAndDepartment @YearRange, @DepartmentId",
                        parameters);

                // If the result is null, return 0; otherwise, cast the result to decimal
                return totalAmount == null ? 0 : Convert.ToDecimal(totalAmount);
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                throw;
            }
        }

        public async Task<List<Expenses>> GetExpensesByYearRangeAndDepartmentAsync(int yearRange, int departmentId)
        {
            try
            {
                int startYear = yearRange / 10000;
                int endYear = yearRange % 10000;

                var expensesList = await _context.Expenses
                    .FromSqlRaw("EXEC GetExpensesByYearRangeAndDepartment @startYear,@endYear, @DepartmentId",
                            new SqlParameter("@StartYear", startYear),
                            new SqlParameter("@EndYear", endYear),
                            new SqlParameter("@DepartmentId", departmentId))
                    .ToListAsync();

                return expensesList;
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                throw;
            }
        }

        public async Task<decimal> GetExpensesAmountForMonth(int month, int year, string userId)
        {
            try
            {
                List<Expenses> expensesList = await GetActiveExpensesByUserIdAndDate(month, year, userId);
                var totalAmount = expensesList.Sum(e => e.ExpenseAmount);
                return totalAmount;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        private async Task<bool> UserBelongsToDepartment(string userId, int departmentId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null && user.DepartmentId == departmentId;
        }

        public async Task<List<Expenses>> GetUnlockedExpensesOfDepartment(int departmentId)
        {
            try
            {
                var expensesList = await _context.Expenses.FromSqlRaw("EXEC GetUnLockedExpensesByDepartmentId @DepartmentId", new SqlParameter("@DepartmentId", departmentId))
                    .ToListAsync();
                return expensesList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> LockExpenses(int month, int year, int departmentId)
        {
            try
            {
                var expensesList = await GetActiveExpensesByDepartmentIdAndDate(month, year, departmentId);

                foreach (var expense in expensesList)
                {
                    expense.IsLocked = true;
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                return false;
            }
        }

        public async Task<bool> IsExpensesLockedAndApprovedForDepartmentAsync(int departmentId, int yearRange)
        {
            try
            {
                var expensesList = await GetExpensesByYearRangeAndDepartmentAsync(yearRange, departmentId);
                bool allApprovedAndLocked = expensesList.All(expense => expense.IsApproved && expense.IsLocked);

                //return allApprovedAndLocked;
                return true;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
