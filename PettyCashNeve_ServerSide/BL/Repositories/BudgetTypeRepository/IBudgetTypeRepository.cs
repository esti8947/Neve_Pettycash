using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.BudgetTypeRepository
{
    public interface IBudgetTypeRepository
    {
        public Task<List<BudgetType>> GetBudgetTypesAsync();
        public Task<bool> CreateBudgetTypeAsync(BudgetType newBudgetType);
        public Task<bool> UpdateBudgetTypeAsync(BudgetType updatedBudgetType);
        public Task<BudgetType> GetBudgetTypeByIdAsync(int id);
    }
}
