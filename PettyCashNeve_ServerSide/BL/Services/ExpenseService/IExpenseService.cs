﻿using Entities.Models_Dto.ExpenseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.ExpenseService
{
    public interface IExpenseService
    {
        Task<ServiceResponse<List<ExpenseDto>>> GetAllExpensesAsync();
        Task<ServiceResponse<List<ExpenseDto>>> GetExpensesOfUserAsync(string updatedBy);
        Task<ServiceResponse<List<ExpenseDto>>> GetExpensesOfUserByYear(string updatedBy, int year);
        Task<ServiceResponse<List<ExpenseDto>>> GetExpensesOfDepartmentByYear(int departmentId, int year);
        Task<ServiceResponse<List<ExpenseDto>>> GetUnapprovedExpensesByUserAsync(string updatedBy);
        Task<ServiceResponse<List<ExpenseDto>>> GetApprovedAndUnlockedExpensesOfDepartment(int departmentId);
        Task<ServiceResponse<bool>> UpdateExpenseAsync(ExpenseDto updatedExpense);
        Task<ServiceResponse<bool>> DeleteExpenseAsync(int id);
        Task<ServiceResponse<bool>> CreateExpenseAsync(ExpenseDto expenseDto);
        Task<ServiceResponse<bool>> ApproveExpenses(List<ExpenseDto> expenses);
        Task<ServiceResponse<bool>> ApproveAllExpenses(string userId, int year, int month);
        Task<ServiceResponse<bool>> LockExpenses(int month, int year, int departmentId);
        Task<ServiceResponse<List<ExpenseDto>>> GetExpensesOfDepartmentAsync(int departmentId);
        Task<ServiceResponse<decimal>> GetExpensesAmountForMonth(int month, int year, string userId);
        Task<ServiceResponse<decimal>> GetExpensesAmountForMonthByDepartmentId(int month, int year, int departmentId);
        Task<ServiceResponse<decimal>> GetExpensesAmountForAcademicYear(int month, int year, int departmentId);
        Task<ServiceResponse<bool>> IsExpensesLockedAndApprovedForDepartmentAsync(int departmentId, int yearRange);


    }
}
