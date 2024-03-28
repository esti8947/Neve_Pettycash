using PettyCashNeve_ServerSide.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.ExpenseCategoryService
{
    public interface IExpenseCategoryService
    {
        Task<ServiceResponse<List<ExpenseCategoryDto>>> GetAllExpenseCategoriesAsync();
        Task<ServiceResponse<bool>> CreateExpenseCategory(ExpenseCategoryDto expenseCategoryDto);
        Task<ServiceResponse<bool>> DeleteExpenseCategory(int expenseCategoryId);
        Task<ServiceResponse<bool>> UpdateExpenseCategory(ExpenseCategoryDto updatedExpenseCategory);

    }
}
