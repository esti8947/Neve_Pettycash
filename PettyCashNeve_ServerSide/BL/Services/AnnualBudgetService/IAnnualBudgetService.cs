using DAL.Models;
using Entities.Models_Dto.BudgetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.AnnualBudgetService
{
    public interface IAnnualBudgetService
    {
        Task<ServiceResponse<List<AnnualBudgetDto>>> GetAnnualBudgetsByDepartmentIdAsync(int departmentId);

        Task<ServiceResponse<AnnualBudgetDto>> GetAnnualBudgetByDepartmentIdAndIsActiveAsync(int departmentId);

        Task<ServiceResponse<AnnualBudgetDto>> CreateAnnualBudgetAsync(AnnualBudgetDto annualBudgetDto);

        Task<ServiceResponse<bool>> AddSumToAnnualBudgetCeilingAsync(int annualBudgetId, int additionalAmount);

        Task<ServiceResponse<bool>> DeleteAnnualBudgetAsync(int annualBudgetId);
    }
}
