using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.RefundBudgetRepository
{
    public interface IRefundBudgetRepository
    {
        Task<RefundBudget> CreateRefundBudgetAsync(RefundBudget refundBudget);

        Task<List<RefundBudget>> GetRefundBudgetsByDepartmentIdAsync(int departmentId);

        Task<RefundBudget> GetRefundBudgetByDepartmentIdAndIsActiveAsync(int departmentId);
        Task<bool> deactivateRefundBudget(int departmentId);
        Task<bool> DeleteRefundBudgetAsync(int refundBudgetId);
    }
}
