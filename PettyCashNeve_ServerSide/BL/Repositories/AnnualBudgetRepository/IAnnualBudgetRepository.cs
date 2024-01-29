using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.AnnualBudgetRepository
{
    public interface IAnnualBudgetRepository
    {
        Task<AnnualBudget> CreateAnnualBudgetAsync(AnnualBudget annualBudget);

        Task<List<AnnualBudget>> GetAnnualBudgetsByDepartmentIdAsync(int departmentId);

        Task<AnnualBudget> GetAnnualBudgetsByDepartmentIdAndIsActiveAsync(int departmentId);

        Task<bool> AddSumToAnnualBudgetCeilingAsync(int annualBudgetId, int additionalAmount);

        Task<bool> DeleteAnnualBudgetAsync(int annualBudgetId);
    }
}
