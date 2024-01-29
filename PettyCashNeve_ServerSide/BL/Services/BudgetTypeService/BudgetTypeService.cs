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
            //var expensesList = await _expenseRepository.GetExpensesOfDepartment(departmentId);
            try
            {
                var department = await _departmentRepository.GetDepartmentByIdAsync(departmentId);
                int budgetTypeId = department?.CurrentBudgetTypeId ?? 0;
                var budgetType = (await GetBudgetTypeByIdAsync(budgetTypeId))?.Data;
                budgetInformation.BudgetType = budgetType;

                if (budgetType.BudgetTypeId == 1)
                {
                    var activeAnnualBudgetResponse = await _annualBudgetService.GetAnnualBudgetByDepartmentIdAndIsActiveAsync(departmentId);
                    var activeAnnualBudget = activeAnnualBudgetResponse.Data;
                    budgetInformation.AnnualBudget = activeAnnualBudget;

                    if (activeAnnualBudget != null)
                    {
                        int yearRange = activeAnnualBudget.AnnualBudgetYear;
                        var expensesOfYer = await _expenseRepository.GetExpensesByYearRangeAndDepartmentAsync(yearRange, departmentId);
                        decimal totalAmount = expensesOfYer.Select(e => e.ExpenseAmount).Sum();

                        // Assign the total amount to the AnnualBudget property
                        budgetInformation.TotalAmount = totalAmount;
                    }
                }
                else
                {
                    if (budgetType.BudgetTypeId == 2)
                    {
                        var activeMonthlyBudget = (await _monthlyBudgetService.GetMonthlyBudgetByDepartmentIdAndIsActiveAsync(departmentId))?.Data;
                        budgetInformation.MonthlyBudget = activeMonthlyBudget;
                        var expensesOfMonth = await _expenseRepository.GetActiveExpensesByDepartmentIdAndDate(activeMonthlyBudget.MonthlyBudgetMonth, activeMonthlyBudget.MonthlyBudgetYear, departmentId);

                        var totalAmount = expensesOfMonth.Select(e => e.ExpenseAmount).Sum();
                        budgetInformation.TotalAmount = totalAmount;
                    }
                    else
                    {
                        var activeRefundBudget = (await _refundBudgetService.GetRefundBudgetByDepartmentIdAndIsActiveAsync(departmentId))?.Data;
                        budgetInformation.RefundBudget = activeRefundBudget;
                        int yearRang = activeRefundBudget.RefundBudgetYear;
                        var expensesOfYear = await _expenseRepository.GetExpensesByYearRangeAndDepartmentAsync(yearRang, departmentId);
                        var totalAmount = expensesOfYear.Select(e => e.ExpenseAmount).Sum();

                        budgetInformation.TotalAmount = totalAmount;
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            serviceResponse.Data = budgetInformation;
            return serviceResponse;
        }

        public Task<ServiceResponse<object>> GetBudgetInformationByDepartmentId(int? departmentIdByAdmin)
        {
            throw new NotImplementedException();
        }
    }

}
