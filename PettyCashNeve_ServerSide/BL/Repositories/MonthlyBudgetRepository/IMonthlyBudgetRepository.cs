using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.MonthlyBudgetRepository
{
    public interface IMonthlyBudgetRepository
    {
        Task<MonthlyBudget> CreateMonthlyBudgetAsync(MonthlyBudget monthlyBudget);

        Task<List<MonthlyBudget>> GetMonthlyBudgetsByDepartmentIdAsync(int departmentId);

        Task<MonthlyBudget> GetMonthlyBudgetsByDepartmentIdAndIsActiveAsync(int departmentId);

        Task<bool> AddSumToMonthlyBudgetCeilingAsync(int monthlyBudgetId, int additionalAmount);
        Task<bool> deactivateMonthlyBudget(int departmentId);    
        Task<bool> DeleteMonthlyBudgetAsync(int monthlyBudgetId);
        Task<bool> resettingMonthlyBudget(int departmentId);
        Task<bool> addAmountToMonthlyBudget(int departmentId, decimal amountToAdd);

    }
}
