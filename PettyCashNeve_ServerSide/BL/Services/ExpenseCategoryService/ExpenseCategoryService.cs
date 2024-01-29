using AutoMapper;
using BL.Repositories.ExpenseCategoryRepository;
using DAL.Models;
using PettyCashNeve_ServerSide.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.ExpenseCategoryService
{
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;
        private readonly IMapper _mapper;
        public ExpenseCategoryService(IExpenseCategoryRepository expenseCategoryRepository, IMapper mapper)
        {
            _expenseCategoryRepository = expenseCategoryRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<bool>> CreateExpenseCategory(ExpenseCategoryDto expenseCategoryDto)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var expenseCategory = _mapper.Map<ExpenseCategory>(expenseCategoryDto);
                var result = await _expenseCategoryRepository.CreateExpenseCategoryAsync(expenseCategory);
                serviceResponse.Success = result;
                serviceResponse.Data = result;
                if (!result)
                {
                    serviceResponse.Message = "ExpenseCategory creation failed";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeleteExpenseCategory(int expenseCategoryId)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var result = await _expenseCategoryRepository.DeleteExpenseCategory(expenseCategoryId);
                serviceResponse.Success = result;
                serviceResponse.Data = result;
            }
            catch (Exception ex)
            {
                serviceResponse.Success= false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ExpenseCategoryDto>>> GetAllExpenseCategoriesAsync()
        {
            var serviceResponse = new ServiceResponse<List<ExpenseCategoryDto>>();
            try
            {
                var expensesCategory = await _expenseCategoryRepository.GetAllExpenseCategoryAsync();
                var expensesCategoryDtos =_mapper.Map<List<ExpenseCategoryDto>>(expensesCategory);
                serviceResponse.Data = expensesCategoryDtos;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
