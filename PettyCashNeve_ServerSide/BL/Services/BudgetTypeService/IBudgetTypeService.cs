using Entities.Models_Dto.BudgetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.BudgetTypeService
{
    public interface IBudgetTypeService
    {
        public Task<ServiceResponse<List<BudgetTypeDto>>> GetBudgetTypes();
        public Task<ServiceResponse<bool>> CreateBudgetType(BudgetTypeDto budgetTypeDto);
        public Task<ServiceResponse<bool>> UpdateBudgetType(BudgetTypeDto budgetTypeDto);
        public Task<ServiceResponse<BudgetTypeDto>> GetBudgetTypeByIdAsync(int id);
        public Task<ServiceResponse<BudgetInformation>> GetBudgetInformationByDepartmentId(int departmentId);
        Task<ServiceResponse<object>> GetBudgetInformationByDepartmentId(int? departmentIdByAdmin);
    }
}
