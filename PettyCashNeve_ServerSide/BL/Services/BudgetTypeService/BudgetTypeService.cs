using AutoMapper;
using BL.Repositories.AnnualBudgetRepository;
using BL.Repositories.BudgetTypeRepository;
using BL.Repositories.ExpenseRepository;
using BL.Services.AnnualBudgetService;
using BL.Services.MonthlyBudgetService;
using BL.Services.RefundBudgetService;
using DAL.Models;
using Entities.Models_Dto.BudgetDto;
using Microsoft.EntityFrameworkCore;
using PettyCashNeve_ServerSide.Dto;
using PettyCashNeve_ServerSide.Repositories.DepartmentRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BL.Services.BudgetTypeService
{
    public class BudgetTypeService : IBudgetTypeService
    {
        private readonly IBudgetTypeRepository _budgetTypeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IAnnualBudgetService _annualBudgetService;
        private readonly IMonthlyBudgetService _monthlyBudgetService;
        private readonly IRefundBudgetService _refundBudgetService;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;

        public BudgetTypeService(IBudgetTypeRepository budgetTypeRepository, IDepartmentRepository departmentRepository,
                IAnnualBudgetService annualBudgetService, IMonthlyBudgetService monthlyBudgetService,
                IExpenseRepository expenseRepository, IRefundBudgetService refundBudgetService, IMapper mapper)
        {
            _budgetTypeRepository = budgetTypeRepository;
            _departmentRepository = departmentRepository;
            _annualBudgetService = annualBudgetService;
            _monthlyBudgetService = monthlyBudgetService;
            _refundBudgetService = refundBudgetService;
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<BudgetTypeDto>>> GetBudgetTypes()
        {
            var serviceResponse = new ServiceResponse<List<BudgetTypeDto>>();
            try
            {
                var budgetTypes = await _budgetTypeRepository.GetBudgetTypesAsync();
                var budgetTypesDto = _mapper.Map<List<BudgetTypeDto>>(budgetTypes);
                if (budgetTypesDto.Count == 0)
                {
                    serviceResponse.Message = "no budgetType found";
                }

                serviceResponse.Data = budgetTypesDto;


            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> CreateBudgetType(BudgetTypeDto budgetTypeDto)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var newBudgetType = _mapper.Map<BudgetType>(budgetTypeDto);
                var result = await _budgetTypeRepository.CreateBudgetTypeAsync(newBudgetType);
                serviceResponse.Data = result;
                serviceResponse.Success = result;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> UpdateBudgetType(BudgetTypeDto budgetTypeDto)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var updatedBudgetType = _mapper.Map<BudgetType>(budgetTypeDto);
                var result = await _budgetTypeRepository.UpdateBudgetTypeAsync(updatedBudgetType);
                serviceResponse.Data = result;
                serviceResponse.Success = result;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<BudgetTypeDto>> GetBudgetTypeByIdAsync(int id)
        {
            var serviceResponse = new ServiceResponse<BudgetTypeDto>();
            try
            {
                var budgetType = await _budgetTypeRepository.GetBudgetTypeByIdAsync(id);
                var budgetTypeDto = _mapper.Map<BudgetTypeDto>(budgetType);
                if (budgetTypeDto == null)
                {
                    serviceResponse.Message = "no budgetType found";
                }

                serviceResponse.Data = budgetTypeDto;


            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }


        public async Task<ServiceResponse<BudgetInformation>> GetBudgetInformationByDepartmentId(int departmentId)
        {
            var serviceResponse = new ServiceResponse<BudgetInformation>();
            var budgetInformation = new BudgetInformation();

            try
            {
                var department = await _departmentRepository.GetDepartmentByIdAsync(departmentId);
                if (department == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Department not found.";
                    return serviceResponse;
                }

                var budgetType = await GetBudgetTypeByDepartmentId(departmentId);
                if (budgetType == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No active budget type found.";
                    return serviceResponse;
                }

                budgetInformation.BudgetType = budgetType;

                switch (budgetType.BudgetTypeId)
                {
                    case 1:
                        var annualBudgetResponse = await _annualBudgetService.GetAnnualBudgetByDepartmentIdAndIsActiveAsync(departmentId);
                        if (!annualBudgetResponse.Success)
                        {
                            serviceResponse.Success = false;
                            serviceResponse.Message = annualBudgetResponse.Message;
                            return serviceResponse;
                        }

                        budgetInformation.AnnualBudget = annualBudgetResponse.Data;
                        budgetInformation.TotalAmount = await CalculateTotalAmountForAnnualBudget(departmentId, annualBudgetResponse.Data);
                        break;

                    case 2:
                        var monthlyBudgetResponse = await _monthlyBudgetService.GetMonthlyBudgetByDepartmentIdAndIsActiveAsync(departmentId);
                        if (!monthlyBudgetResponse.Success)
                        {
                            serviceResponse.Success = false;
                            serviceResponse.Message = monthlyBudgetResponse.Message;
                            return serviceResponse;
                        }

                        budgetInformation.MonthlyBudget = monthlyBudgetResponse.Data;
                        budgetInformation.TotalAmount = await CalculateTotalAmountForMonthlyBudget(departmentId, monthlyBudgetResponse.Data);
                        break;

                    case 3:
                        var refundBudgetResponse = await _refundBudgetService.GetRefundBudgetByDepartmentIdAndIsActiveAsync(departmentId);
                        if (!refundBudgetResponse.Success)
                        {
                            serviceResponse.Success = false;
                            serviceResponse.Message = refundBudgetResponse.Message;
                            return serviceResponse;
                        }

                        budgetInformation.RefundBudget = refundBudgetResponse.Data;
                        budgetInformation.TotalAmount = await CalculateTotalAmountForRefundBudget(departmentId, refundBudgetResponse.Data);
                        break;

                    default:
                        serviceResponse.Success = false;
                        serviceResponse.Message = "No active budget found.";
                        break;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            serviceResponse.Data = budgetInformation;
            return serviceResponse;
        }

        public Task<ServiceResponse<object>> GetBudgetInformationByDepartmentId(int? departmentIdByAdmin)
        {
            throw new NotImplementedException();
        }

        private async Task<BudgetTypeDto> GetBudgetTypeByDepartmentId(int departmentId)
        {
            var department = await _departmentRepository.GetDepartmentByIdAsync(departmentId);
            if (department == null)
                return null;

            int budgetTypeId = department.CurrentBudgetTypeId;
            return (await GetBudgetTypeByIdAsync(budgetTypeId))?.Data;
        }

        private async Task<decimal> CalculateTotalAmountForAnnualBudget(int departmentId, AnnualBudgetDto annualBudget)
        {
            if (annualBudget == null)
                return 0;

            var expensesOfYear = await _expenseRepository.GetExpensesByYearRangeAndDepartmentAsync(annualBudget.AnnualBudgetYear, departmentId);
            return expensesOfYear.Sum(e => e.ExpenseAmount);
        }

        private async Task<decimal> CalculateTotalAmountForMonthlyBudget(int departmentId, MonthlyBudgetDto monthlyBudget)
        {
            if (monthlyBudget == null)
                return 0;

            int targetYear = monthlyBudget.MonthlyBudgetYear;

            if (monthlyBudget.MonthlyBudgetMonth > 8)
            {
                targetYear = monthlyBudget.MonthlyBudgetYear / 10000;
            }
            else
            {
                targetYear = monthlyBudget.MonthlyBudgetYear % 10000;
            }

            var expensesOfMonth = await _expenseRepository.GetActiveExpensesByDepartmentIdAndDate(monthlyBudget.MonthlyBudgetMonth, targetYear, departmentId);
            return expensesOfMonth.Sum(e => e.ExpenseAmount);
        }

        private async Task<decimal> CalculateTotalAmountForRefundBudget(int departmentId, RefundBudgetDto refundBudget)
        {
            if (refundBudget == null)
                return 0;

            var expensesOfYear = await _expenseRepository.GetExpensesByYearRangeAndDepartmentAsync(refundBudget.RefundBudgetYear, departmentId);
            return expensesOfYear.Sum(e => e.ExpenseAmount);
        }
    }

}
