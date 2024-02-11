using AutoMapper;
using BL.Repositories.MonthlyBudgetRepository;
using DAL.Models;
using Entities.Models_Dto.BudgetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.MonthlyBudgetService
{
    public class MonthlyBudgetService : IMonthlyBudgetService
    {
        private readonly IMonthlyBudgetRepository _monthlyBudgetRepository;
        private readonly IMapper _mapper;

        public MonthlyBudgetService(IMonthlyBudgetRepository monthlyBudgetRepository, IMapper mapper)
        {
            _monthlyBudgetRepository = monthlyBudgetRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<MonthlyBudgetDto>>> GetMonthlyBudgetsByDepartmentIdAsync(int departmentId)
        {
            var serviceResponse = new ServiceResponse<List<MonthlyBudgetDto>>();
            try
            {
                var monthlyBudgets = await _monthlyBudgetRepository.GetMonthlyBudgetsByDepartmentIdAsync(departmentId);
                var monthlyBudgetDtos = _mapper.Map<List<MonthlyBudgetDto>>(monthlyBudgets);
                serviceResponse.Data = monthlyBudgetDtos;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<MonthlyBudgetDto>> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<MonthlyBudgetDto>> GetMonthlyBudgetByDepartmentIdAndIsActiveAsync(int departmentId)
        {
            var serviceResponse = new ServiceResponse<MonthlyBudgetDto>();
            try
            {
                var monthlyBudget = await _monthlyBudgetRepository.GetMonthlyBudgetsByDepartmentIdAndIsActiveAsync(departmentId);
                var monthlyBudgetDto = _mapper.Map<MonthlyBudgetDto>(monthlyBudget);
                serviceResponse.Data = monthlyBudgetDto;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                return new ServiceResponse<MonthlyBudgetDto> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<MonthlyBudgetDto>> CreateMonthlyBudgetAsync(MonthlyBudgetDto monthlyBudgetDto)
        {
            try
            {
                var monthlyBudget = _mapper.Map<MonthlyBudget>(monthlyBudgetDto);
                var createdMonthlyBudget = await _monthlyBudgetRepository.CreateMonthlyBudgetAsync(monthlyBudget);
                var createdMonthlyBudgetDto = _mapper.Map<MonthlyBudgetDto>(createdMonthlyBudget);
                return new ServiceResponse<MonthlyBudgetDto> { Data = createdMonthlyBudgetDto };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<MonthlyBudgetDto> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<bool>> AddSumToMonthlyBudgetCeilingAsync(int monthlyBudgetId, int additionalAmount)
        {
            try
            {
                var result = await _monthlyBudgetRepository.AddSumToMonthlyBudgetCeilingAsync(monthlyBudgetId, additionalAmount);
                return new ServiceResponse<bool> { Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<bool>> DeleteMonthlyBudgetAsync(int monthlyBudgetId)
        {
            try
            {
                var result = await _monthlyBudgetRepository.DeleteMonthlyBudgetAsync(monthlyBudgetId);
                return new ServiceResponse<bool> { Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<bool>> resettingMonthlyBudget(int departmentId)
        {
            try
            {
                var result = await _monthlyBudgetRepository.resettingMonthlyBudget(departmentId);
                return new ServiceResponse<bool> { Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<bool>> addAmountToMonthlyBudget(int departmentId, decimal amountToAdd)
        {
            try
            {
                var result = await _monthlyBudgetRepository.addAmountToMonthlyBudget(departmentId, amountToAdd);
                return new ServiceResponse<bool> { Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, Message = ex.Message };
            }
        }
    }
}
