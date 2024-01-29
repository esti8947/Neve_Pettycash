using Entities.Models_Dto.BudgetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.RefundBudgetService
{
    public interface IRefundBudgetService
    {
        Task<ServiceResponse<List<RefundBudgetDto>>> GetRefundBudgetsByDepartmentIdAsync(int departmentId);

        Task<ServiceResponse<RefundBudgetDto>> GetRefundBudgetByDepartmentIdAndIsActiveAsync(int departmentId);

        Task<ServiceResponse<RefundBudgetDto>> CreateRefundBudgetAsync(RefundBudgetDto refundBudgetDto);

        Task<ServiceResponse<bool>> DeleteRefundBudgetAsync(int refundBudgetId);
    }
}
