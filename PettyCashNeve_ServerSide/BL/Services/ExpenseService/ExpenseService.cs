using AutoMapper;
using BL.Repositories.ExpenseRepository;
using DAL.Models;
using Entities.Models_Dto.ExpenseDto;
using PettyCashNeve_ServerSide.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.ExpenseService
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        public ExpenseService(IExpenseRepository expenseRepository, IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<ExpenseDto>>> GetAllExpensesAsync()
        {
            var serviceResponse = new ServiceResponse<List<ExpenseDto>>();
            try
            {
                var expenses = await _expenseRepository.GetAllExpensesAsync();
                var expenseDtos = _mapper.Map<List<ExpenseDto>>(expenses);
                serviceResponse.Data = expenseDtos;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ExpenseDto>>> GetExpensesOfUserAsync(string updatedBy)
        {
            var serviceResponse = new ServiceResponse<List<ExpenseDto>>();
            try
            {
                var expensesList = await _expenseRepository.GetExpensesOfUserAsync(updatedBy);
                var expensesListDto = _mapper.Map<List<ExpenseDto>>(expensesList);
                if (expensesListDto != null && expensesListDto.Count > 0)
                {
                    serviceResponse.Data = expensesListDto;
                    serviceResponse.Success = true;

                }
                else
                {
                    serviceResponse.Message = "no expenses fount";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message += ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ExpenseDto>>> GetExpensesOfUserByYear(string updatedBy, int year)
        {
            var serviceResponse = new ServiceResponse<List<ExpenseDto>>();
            try
            {
                var expensesList = await _expenseRepository.GetExpensesOfUserByYear(updatedBy, year);
                var expensesListDto = _mapper.Map<List<ExpenseDto>>(expensesList);
                if (expensesListDto != null && expensesListDto.Count > 0)
                {
                    serviceResponse.Data = expensesListDto;
                    serviceResponse.Success = true;

                }
                else
                {
                    serviceResponse.Message = "no expenses found";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message += ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ExpenseDto>>> GetExpensesOfDepartmentByYear(int departmentId, int year)
        {
            var serviceResponse = new ServiceResponse<List<ExpenseDto>>();
            try
            {
                var expensesList = await _expenseRepository.GetExpensesOfDepartmentByYear(departmentId, year);
                var expensesListDto = _mapper.Map<List<ExpenseDto>>(expensesList);
                if (expensesListDto != null && expensesListDto.Count > 0)
                {
                    serviceResponse.Data = expensesListDto;
                    serviceResponse.Success = true;

                }
                else
                {
                    serviceResponse.Message = "no expenses found";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message += ex.Message;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<List<ExpenseDto>>> GetUnapprovedExpensesByUserAsync(string updatedBy)
        {
            var serviceResponse = new ServiceResponse<List<ExpenseDto>>();
            try
            {
                var expensesList = await _expenseRepository.GetUnapprovedExpensesByUserAsync(updatedBy);
                var expensesListDto = _mapper.Map<List<ExpenseDto>>(expensesList);
                if (expensesListDto != null && expensesListDto.Count > 0)
                {
                    serviceResponse.Data = expensesListDto;
                    serviceResponse.Success = true;

                }
                else
                {
                    serviceResponse.Message = "no expenses fount";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message += ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ExpenseDto>>> GetApprovedAndUnlockedExpensesOfDepartment(int departmentId)
        {
            var serviceResponse = new ServiceResponse<List<ExpenseDto>>();
            try
            {
                var expensesList = await _expenseRepository.GetApprovedAndUnlockedExpensesOfDepartment(departmentId);
                var expensesListDto = _mapper.Map<List<ExpenseDto>>(expensesList);

                  serviceResponse.Data = expensesListDto;
                  serviceResponse.Success = true;


            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message += ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> UpdateExpenseAsync(ExpenseDto updatedExpense)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var expenseToUpdate = _mapper.Map<Expenses>(updatedExpense);
                var result = await _expenseRepository.UpdateExpenseAsync(expenseToUpdate);

                serviceResponse.Data = result;
                serviceResponse.Success = result;
            }
            catch (NotFoundException ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            catch (Exception ex)
            {

                throw;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeleteExpenseAsync(int expenseId)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var result = await _expenseRepository.DeleteExpenseAsync(expenseId);

                serviceResponse.Data = result;
                serviceResponse.Success = true;
            }
            catch (NotFoundException ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> CreateExpenseAsync(ExpenseDto expenseDto)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var newExpense = _mapper.Map<Expenses>(expenseDto);
                var result = await _expenseRepository.CreateExpenseAsync(newExpense);
                serviceResponse.Success = result;
                serviceResponse.Data = result;

                if (!result)
                {
                    serviceResponse.Message = "Expanse creation failed";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> ApproveExpenses(List<ExpenseDto> expenses)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var expensesList = _mapper.Map<List<Expenses>>(expenses);
                var result = await _expenseRepository.ApproveExpenses(expensesList);
                serviceResponse.Success = result;
                serviceResponse.Data = result;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> ApproveAllExpenses(string userId, int year, int month)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var result = await _expenseRepository.ApproveAllExpenses(userId,year, month);
                serviceResponse.Success = result;
                serviceResponse.Data = result;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ExpenseDto>>> GetExpensesOfDepartmentAsync(int departmentId)
        {
            var serviceResponse = new ServiceResponse<List<ExpenseDto>>();
            try
            {
                var expensesList = await _expenseRepository.GetExpensesOfDepartment(departmentId);
                var expensesListDto = _mapper.Map<List<ExpenseDto>>(expensesList);

                if (expensesListDto != null && expensesListDto.Count > 0)
                {
                    serviceResponse.Data = expensesListDto;
                    serviceResponse.Success = true;
                }
                else
                {
                    serviceResponse.Message = "No expenses found for the specified department.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message += ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<decimal>> GetExpensesAmountForMonth(int month, int year, string userId)
        {
            var serviceResponse = new ServiceResponse<decimal>();
            try
            {
                var totalAmount = await _expenseRepository.GetExpensesAmountForMonth(month, year, userId);
                if (totalAmount != null)
                {
                    serviceResponse.Data = totalAmount;
                    serviceResponse.Success = true;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> LockExpenses(int month, int year,int departmentId)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var result = await _expenseRepository.LockExpenses(month, year, departmentId);
                serviceResponse.Success = result;
                serviceResponse.Data = result;
            }
            catch (Exception ex)
            {
                serviceResponse.Success=false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
