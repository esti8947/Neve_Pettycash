using AutoMapper;
using BL.Repositories.BuyerRepository;
using BL.Repositories.EventCategoryRepository;
using BL.Repositories.EventRepository;
using BL.Repositories.ExpenseCategoryRepository;
using BL.Repositories.ExpenseRepository;
using BL.Services.EventCategoryService;
using BL.Services.UserService;
using DAL.Models;
using Entities.Models_Dto.ExpenseDto;
using PettyCashNeve_ServerSide.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.ExpenseService
{
    public class ExpenseMoreInfoService : IExpenseMoreInfoService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IExpenseCategoryRepository _expenseCategory;
        private readonly IBuyerRepository _buyerRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public ExpenseMoreInfoService(IExpenseRepository expenseRepository,IBuyerRepository buyerRepository, IEventCategoryRepository eventCategoryRepository, 
            IEventRepository eventRepository,IExpenseCategoryRepository expenseCategory, IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _expenseCategory = expenseCategory;
            _buyerRepository = buyerRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        //private async Task<List<ExpenseReportInfoDto>> GetExpenseReportList()
        //{
        //    var expenses = await _expenseRepository.GetAllExpensesAsync();
        //    var expenseReportInfoDtos = new List<ExpenseReportInfoDto>();

        //    foreach (var expense in expenses)
        //    {
        //        var expenseReportInfoDto = new ExpenseReportInfoDto
        //        {
        //            Expense = _mapper.Map<ExpenseDto>(expense),
        //            ExpenseCategoryName = await _expenseCategory.GetExpenseCategoryNameByIdAsync(expense.ExpenseCategoryId),
        //            CategoryName = await _eventCategoryRepository.GetEventCategoryNameByIdAsync(expense.EventsId),
        //            EventName = await _eventRepository.GetEventNameById(expense.EventsId),
        //            BuyerName = await _buyerRepository.GetBuyerNameByIdAsync(expense.BuyerId ?? 0),
        //            //UserName = await _userRepository.GetUserNameByIdAsync(userId)
        //        };

        //        expenseReportInfoDtos.Add(expenseReportInfoDto);
        //    }
        //    return expenseReportInfoDtos;
        //}

        private async Task<List<ExpenseReportInfoDto>> GetExpenseMoreInfoList(List<Expenses> expenses)
        {
            var expenseMoreInfoDtos = new List<ExpenseReportInfoDto>();

            foreach (var expense in expenses)
            {
                var expenseMoreInfoDto = new ExpenseReportInfoDto
                {
                    Expense = _mapper.Map<ExpenseDto>(expense),
                    ExpenseCategoryName = await _expenseCategory.GetExpenseCategoryNameByIdAsync(expense.ExpenseCategoryId),
                    CategoryName = await _eventCategoryRepository.GetEventCategoryNameByIdAsync(expense.EventsId),
                    EventName = await _eventRepository.GetEventNameById(expense.EventsId),
                    BuyerName = await _buyerRepository.GetBuyerNameByIdAsync(expense.BuyerId ?? 0),
                    // Add more properties as needed
                };

                expenseMoreInfoDtos.Add(expenseMoreInfoDto);
            }

            return expenseMoreInfoDtos;
        }


        //public async Task<ServiceResponse<List<ExpenseReportInfoDto>>> GetExpenseReportInfoAsync()
        //{
        //    var serviceResponse = new ServiceResponse<List<ExpenseReportInfoDto>>();

        //    try
        //    {
        //        var expenseReportInfoDtos = await GetExpenseReportList();
        //        serviceResponse.Data = expenseReportInfoDtos;
        //        serviceResponse.Success = true;
        //    }
        //    catch (Exception ex)
        //    {

        //        serviceResponse.Success = false;
        //        serviceResponse.Message = ex.Message;
        //    }

        //    return serviceResponse;
        //}

        public async Task<ServiceResponse<List<ExpenseReportInfoDto>>> GetExpensesReportOfUser(string updatedBy)
        {
            var serviceResponse = new ServiceResponse<List<ExpenseReportInfoDto>>();
            try
            {
                var expensesList = await _expenseRepository.GetExpensesOfUserAsync(updatedBy);

                var expenseOfUser = await GetExpenseMoreInfoList(expensesList);
                serviceResponse.Data = expenseOfUser;
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ExpenseReportInfoDto>>> GetExpensesReportOfDepartment(int departmentId)
        { 
            var serviceResponse = new ServiceResponse<List<ExpenseReportInfoDto>>();
            try
            {
                var expensesList = await _expenseRepository.GetExpensesOfDepartment(departmentId);

                var expenseOfUser = await GetExpenseMoreInfoList(expensesList);
                serviceResponse.Data = expenseOfUser;
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        //public async Task<ServiceResponse<List<ExpenseReportInfoDto>>> GetExpensesReportOfCurrentMonth(string updatedBy, int currentMonth)
        //{
        //    var serviceResponse = new ServiceResponse<List<ExpenseReportInfoDto>>();
        //    try
        //    {


        //        var expenseReportInfoDtos = await GetExpenseReportList();
        //        var expenseOfUser = expenseReportInfoDtos.Where(e => e.Expense.UpdatedBy.Equals(updatedBy) && e.Expense.RefundMonth.Equals(currentMonth) && e.Expense.IsApproved == false).ToList();
        //        serviceResponse.Data = expenseOfUser;
        //        serviceResponse.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResponse.Success = false;
        //        serviceResponse.Message = ex.Message;
        //    }
        //    return serviceResponse;
        //}

        public async Task<ServiceResponse<List<ExpenseReportInfoDto>>> GetExpensesByYearAndMonth(string updatedBy, int year, int month)
        {
            var serviceResponse = new ServiceResponse<List<ExpenseReportInfoDto>>();
            try
            {
                var expenseList = await _expenseRepository.GetActiveExpensesByUserIdAndDate(month, year, updatedBy);

                var expensesByYearAndMonth = await GetExpenseMoreInfoList(expenseList);
                serviceResponse.Data = expensesByYearAndMonth;
                serviceResponse.Success = true;

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ExpenseReportInfoDto>>> GetExpensesOfDepartmentByYearAndMonth(int departmetnId, int year, int month)
        {
            var serviceResponse = new ServiceResponse<List<ExpenseReportInfoDto>>();
            try
            {
                var expenseList = await _expenseRepository.GetActiveExpensesByDepartmentIdAndDate(month, year, departmetnId);

                var expensesByYearAndMonth = await GetExpenseMoreInfoList(expenseList);
                serviceResponse.Data = expensesByYearAndMonth;
                serviceResponse.Success = true;

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
