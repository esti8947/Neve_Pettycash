using AutoMapper;
using DAL.Models;
using Entities.Models_Dto;
using Entities.Models_Dto.BudgetDto;
using Entities.Models_Dto.ExpenseDto;
using Entities.Models_Dto.UserDto;
using PettyCashNeve_ServerSide.Dto;

namespace PettyCashNeve_ServerSide
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentDto, Department>();
            CreateMap<MonthlyCashRegister, MonthlyCashRegisterDto>();
            CreateMap<MonthlyCashRegisterDto, MonthlyCashRegister>();
            CreateMap<AnnualBudget, AnnualBudgetDto>();
            CreateMap<AnnualBudgetDto, AnnualBudget>();
            CreateMap<RefundBudget, RefundBudgetDto>();
            CreateMap<RefundBudgetDto, RefundBudget>();
            CreateMap<MonthlyBudget, MonthlyBudgetDto>();
            CreateMap<MonthlyBudgetDto, MonthlyBudget>();
            CreateMap<Buyer, BuyerDto>();
            CreateMap<BuyerDto, Buyer>();
            CreateMap<Expenses, ExpenseDto>();
            CreateMap<ExpenseDto, Expenses>();
            CreateMap<Events , EventDto>();
            CreateMap<EventDto, Events>();
            CreateMap<EventCategory, EventCategoryDto>();
            CreateMap<EventCategoryDto, EventCategory>();
            //CreateMap<User, UserDto>();
            //CreateMap<UserDto, User>();
            CreateMap<ExpenseCategory, ExpenseCategoryDto>();
            CreateMap<ExpenseCategoryDto, ExpenseCategory>();
            CreateMap<BudgetType, BudgetTypeDto>();
            CreateMap<BudgetTypeDto, BudgetType>();
        }
    }
}
