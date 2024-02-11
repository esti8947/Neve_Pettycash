using Entities.Models_Dto.BudgetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.MonthlyBudgetService
{
    public interface IMonthlyBudgetService
    {
        Task<ServiceResponse<List<MonthlyBudgetDto>>> GetMonthlyBudgetsByDepartmentIdAsync(int departmentId);

        Task<ServiceResponse<MonthlyBudgetDto>> GetMonthlyBudgetByDepartmentIdAndIsActiveAsync(int departmentId);

        Task<ServiceResponse<MonthlyBudgetDto>> CreateMonthlyBudgetAsync(MonthlyBudgetDto monthlyBudgetDto);

        Task<ServiceResponse<bool>> AddSumToMonthlyBudgetCeilingAsync(int monthlyBudgetId, int additionalAmount);

        Task<ServiceResponse<bool>> DeleteMonthlyBudgetAsync(int monthlyBudgetId);
        Task<ServiceResponse<bool>> resettingMonthlyBudget(int departmentId);
        Task<ServiceResponse<bool>> addAmountToMonthlyBudget(int departmentId, decimal amountToAdd);
    }
}
