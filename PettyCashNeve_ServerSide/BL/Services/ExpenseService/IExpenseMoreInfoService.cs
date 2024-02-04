using Entities.Models_Dto.ExpenseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.ExpenseService
{
    public interface IExpenseMoreInfoService
    {
        //Task<ServiceResponse<List<ExpenseReportInfoDto>>> GetExpenseReportInfoAsync();
        Task<ServiceResponse<List<ExpenseReportInfoDto>>> GetExpensesReportOfUser(string updatedBy);
        //Task<ServiceResponse<List<ExpenseReportInfoDto>>> GetExpensesReportOfCurrentMonth(string updatedBy, int currentMonth);
        Task<ServiceResponse<List<ExpenseReportInfoDto>>> GetExpensesByYearAndMonth(string updatedBy, int year, int month);
        Task<ServiceResponse<List<ExpenseReportInfoDto>>> GetExpensesOfDepartmentByYearAndMonth(int departmetnId, int year, int month);
        Task<ServiceResponse<List<ExpenseReportInfoDto>>> GetUnlockedExpenses(int departmentId);

    }
}
