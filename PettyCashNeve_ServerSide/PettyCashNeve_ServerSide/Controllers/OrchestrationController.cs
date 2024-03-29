﻿using BL.Services;
using BL.Services.AnnualBudgetService;
using BL.Services.EventService;
using BL.Services.ExpenseService;
using BL.Services.MonthlyBudgetService;
using BL.Services.RefundBudgetService;
using DAL.Models;
using Entities.Models_Dto;
using Entities.Models_Dto.BudgetDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PettyCashNeve_ServerSide.Services.DepartmentService;
using PettyCashNeve_ServerSide.Services.MonthlyCashRegisterService;

namespace PettyCashNeve_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrchestrationController : BaseController
    {
        private readonly IEventService _eventService;
        private readonly IExpenseService _expenseService;
        private readonly IMonthlyCashRegisterService _monthlyCashRegisterService;
        private readonly IDepartmentService _departmentService;
        private readonly IAnnualBudgetService _annualBudgetService;
        private readonly IMonthlyBudgetService _monthlyBudgetService;
        private readonly IRefundBudgetService _refundBudgetService;
        public OrchestrationController(IEventService eventService, IExpenseService expenseService, IMonthlyCashRegisterService monthlyCashRegisterService,
                                        IDepartmentService departmentService, IAnnualBudgetService annualBudgetService, IMonthlyBudgetService monthlyBudgetService, IRefundBudgetService refundBudgetService)
        {
            _eventService = eventService;
            _expenseService = expenseService;
            _monthlyCashRegisterService = monthlyCashRegisterService;
            _departmentService = departmentService;
            _annualBudgetService = annualBudgetService;
            _monthlyBudgetService = monthlyBudgetService;
            _refundBudgetService = refundBudgetService;
        }

        [HttpGet("closeMonthlyActivities/{year}/{month}")]
        public async Task<IActionResult> CloseMonthlyActivities(int year, int month)
        {
            try
            {
                var userId = UserId;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserId is null or empty.");
                }

                var deactivateEventsResponse = await _eventService.DeactivateAllEvents();
                var approveExpensesResponse = await _expenseService.ApproveAllExpenses(userId, year, month);
                var deactivateMonthlyCashRegisterResponse = await _monthlyCashRegisterService.DeactivateMonthlyCashRegister(UserId);

                // Check the responses and handle accordingly
                if (deactivateEventsResponse.Success && approveExpensesResponse.Success && deactivateMonthlyCashRegisterResponse.Success)
                {
                    return HandleResponses(deactivateEventsResponse, approveExpensesResponse, deactivateMonthlyCashRegisterResponse);
                }
                else
                {
                    // Handle other scenarios if needed
                    return BadRequest("One or more actions failed.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("resettingBudget/{departmentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResettingBudget(int departmentId)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(departmentId);
            var budgetTypeCode = department.Data.CurrentBudgetTypeId;

            if (budgetTypeCode == 1)
            {
                var serviceResponse = await _annualBudgetService.resettingAnnualBudget(departmentId);
                return HandleResponse(serviceResponse);
            }
            else if (budgetTypeCode == 2)
            {
                var serviceResponse = await _monthlyBudgetService.resettingMonthlyBudget(departmentId);
                return HandleResponse(serviceResponse);
            }
            return BadRequest("One or more actions failed.");
        }

        [HttpGet("addAmountToBudget/{departmentId}/{amountToAdd}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AddAmountToBudget(int departmentId, decimal amountToAdd)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(departmentId);
            var budgetTypeCode = department.Data.CurrentBudgetTypeId;

            if (budgetTypeCode == 1)
            {
                var serviceResponse = await _annualBudgetService.addAmountToAnnualBudget(departmentId, amountToAdd);
                return HandleResponse(serviceResponse);
            }
            else if (budgetTypeCode == 2)
            {
                var serviceResponse = await _monthlyBudgetService.addAmountToMonthlyBudget(departmentId, amountToAdd);
                return HandleResponse(serviceResponse);
            }
            return BadRequest("One or more actions failed.");
        }



        [HttpPost("closeLastYearAndOpenNewYearActivities")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CloseLastYearAndOpenNewYearActivities([FromBody] NewYearModel newYearModel)
        {
            
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var departmentId = newYearModel.DepartmentId;
                var departmentResponse = await _departmentService.GetDepartmentByIdAsync(departmentId);
                var department = departmentResponse.Data;

                if (department == null)
                {
                    return NotFound("Department not found");
                }
                var yaerToCloseResult = _departmentService.GetYearByDepartmentId(departmentId).Result;
                int yearToClose = yaerToCloseResult.Data;
                if (yearToClose == newYearModel.NewYear)
                {
                    return BadRequest("It is not possible to open a new year on a year that already exists.");
                }

                if (yearToClose != 0)
                {
                    var checkRegistersResponse = await _monthlyCashRegisterService.CheckAllMonthlyCashRegistersInactiveForYearAsync(departmentId, yearToClose);
                    var checkExpensesResponse = await _expenseService.IsExpensesLockedAndApprovedForDepartmentAsync(departmentId, yearToClose);

                    if (!checkRegistersResponse.Success || !checkExpensesResponse.Success)
                    {
                        return BadRequest("One or more actions failed");
                    }
                }


                department.CurrentBudgetTypeId = newYearModel.BudgetTypeId;

                var updateResponse = await _departmentService.UpdateDepartmentAsync(department);

                if (!updateResponse.Success)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the department.");
                }
                var deactivateBudget = await _departmentService.deactivateBudget(departmentId);

                switch (department.CurrentBudgetTypeId)
                {
                    case 1:
                        await _annualBudgetService.CreateAnnualBudgetAsync(newYearModel.AnnualBudget);
                        break;
                    case 2:
                        await _monthlyBudgetService.CreateMonthlyBudgetAsync(newYearModel.MonthlyBudget);
                        break;
                    case 3:
                        await _refundBudgetService.CreateRefundBudgetAsync(newYearModel.RefundBudget);
                        break;
                }
                

                serviceResponse.Success = true;
                serviceResponse.Data = true;
                serviceResponse.Message = "New year activities successfully opened.";

                return HandleResponse(serviceResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("getUsersOfDepartment/{departmentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersOfDepartment(int departmentId)
        {
            var serviceResponse = await _departmentService.GetUsersByDepartmentId(departmentId);
            return HandleResponse(serviceResponse);
        }
    }
}

