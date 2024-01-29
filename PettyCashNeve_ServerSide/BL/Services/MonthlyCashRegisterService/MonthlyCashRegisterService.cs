using AutoMapper;
using PettyCashNeve_ServerSide.Dto;
using BL.Services;
using DAL.Models;
using PettyCashNeve_ServerSide.Exceptions;
using PettyCashNeve_ServerSide.Repositories.MonthlyCashRegisterRepository;
using BL.Repositories.ExpenseRepository;
using BL.Repositories.UserRepository;
using Entities.Models_Dto;
using System.Linq;

namespace PettyCashNeve_ServerSide.Services.MonthlyCashRegisterService
{
    public class MonthlyCashRegisterService : IMonthlyCashRegisterService
    {
        private readonly IMonthlyCashRegisterRepository _monthlyCashRegisterRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public MonthlyCashRegisterService(IMonthlyCashRegisterRepository monthlyCashRegisterRepository,
                                            IExpenseRepository expenseRepository, IUserRepository userRepository, IMapper mapper)
        {
            _monthlyCashRegisterRepository = monthlyCashRegisterRepository;
            _expenseRepository = expenseRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<MonthlyCashRegisterDepartmentDto>>> GetCurrentMonthlyCashRegistersByDepartmentIdAsync(int departmentId)
        {
            var serviceResponse = new ServiceResponse<List<MonthlyCashRegisterDepartmentDto>>();
            try
            {
                var usersList = await _userRepository.GetUsersOfDepartment(departmentId);
                var result = new List<MonthlyCashRegisterDepartmentDto>();

                var monthlyRegistersList = new List<MonthlyCashRegister>();
                foreach (var user in usersList)
                {
                    var monthlyRegisters = await _monthlyCashRegisterRepository.GetCurrentMonthlyCashRegistersByUserIdAsync(user.Id);
                    monthlyRegistersList.Add(monthlyRegisters);
                }

                var groupedMonthlyRegisters = monthlyRegistersList
                    .GroupBy(m => m.MonthlyCashRegisterMonth);

                foreach (var group in groupedMonthlyRegisters)
                {
                    decimal totalAmount = group.Sum(m => m.AmountInCashRegister);
                    decimal totalRefundAmount = group.Sum(m => m.RefundAmount);

                    var monthlyRegisterDepartment = new MonthlyCashRegisterDepartmentDto
                    {
                        DepartmentId = departmentId,
                        MonthlyCashRegisterMonth = group.Key,
                        MonthlyCashRegisterYear = group.First().MonthlyCashRegisterYear, // Assuming all items in the group have the same year
                        AmountInCashRegister = totalAmount,
                        RefundAmount = totalRefundAmount
                    };
                    result.Add(monthlyRegisterDepartment);

                }

                serviceResponse.Success = true;
                serviceResponse.Data = result;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Error occurred while retrieving monthly cash registers.";
                throw;
            }
            return serviceResponse;
        }



        public async Task<ServiceResponse<MonthlyCashRegisterDto>> GetCurrentMonthlyCashRegistersByUserIdAsync(string userId)
        {
            var serviceResponse = new ServiceResponse<MonthlyCashRegisterDto>();
            try
            {
                var monthlyRegister = await _monthlyCashRegisterRepository.GetCurrentMonthlyCashRegistersByUserIdAsync(userId);
                var monthlyRegisterDto = _mapper.Map<MonthlyCashRegisterDto>(monthlyRegister);

                serviceResponse.Data = monthlyRegisterDto;
                return serviceResponse;
            }
            catch (Exception ex)
            {

                return new ServiceResponse<MonthlyCashRegisterDto> { Success = false, Message = ex.Message };
            }
        }

        public async Task<int> GetCurrentMonthByUserIdAsync(string userId)
        {
            var monthlyRegister = await _monthlyCashRegisterRepository.GetCurrentMonthlyCashRegistersByUserIdAsync(userId);
            if (monthlyRegister != null)
            {
                return monthlyRegister.MonthlyCashRegisterMonth;
            }
            return 0;
        }


        //public async Task<ServiceResponse<MonthlyCashRegisterDto>> GetMonthlyCashRegistersByUserOfDepartmentIdAsync(int userOfDepartmentId)
        //{
        //    var serviceResponse = new ServiceResponse<MonthlyCashRegisterDto>();
        //    try
        //    {
        //        var monthlyRegister = await _monthlyCashRegisterRepository.GetMonthlyCashRegisterByUserOfDepartmentIdAsync(userOfDepartmentId);
        //        var monthlyRegisterDto = _mapper.Map<MonthlyCashRegisterDto>(monthlyRegister);
        //        serviceResponse.Data = monthlyRegisterDto;
        //        return serviceResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ServiceResponse<MonthlyCashRegisterDto> { Success = false, Message = ex.Message };
        //    }
        //}

        //public async Task<ServiceResponse<List<MonthlyCashRegisterDto>>> GetMonthlyCashRegistersByDepartmentIdAsync(int departmentId)
        //{
        //    var serviceResponse = new ServiceResponse<List<MonthlyCashRegisterDto>>();
        //    try
        //    {
        //        var monthlyRegisters = await _monthlyCashRegisterRepository.GetMonthlyCashRegistersByDepartmentIdAsync(departmentId);
        //        var monthlyRegistersDtos = _mapper.Map<List<MonthlyCashRegisterDto>>(monthlyRegisters);
        //        serviceResponse.Data = monthlyRegistersDtos;
        //        return serviceResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ServiceResponse<List<MonthlyCashRegisterDto>> { Success = false, Message = ex.Message };
        //    }
        //}

        public async Task<ServiceResponse<bool>> CreateNewMonthlyCashRegisterAsync(MonthlyCashRegisterDto newMonthlyCashRegisterDto)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var monthlyCashRegister = _mapper.Map<MonthlyCashRegister>(newMonthlyCashRegisterDto);
                var result = await _monthlyCashRegisterRepository.CreateNewMonthlyCashRegisterAsync(monthlyCashRegister);
                serviceResponse.Data = result;
                serviceResponse.Success = result;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<bool>> UpdateMonthlyCashRegisterAsync(MonthlyCashRegisterDto updateMonthlyCashRegisterDto)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var existingMonthlyCashRegister = _mapper.Map<MonthlyCashRegister>(updateMonthlyCashRegisterDto);
                var result = await _monthlyCashRegisterRepository.UpdateMonthlyCashRegisterAsync(existingMonthlyCashRegister);

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
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeactivateMonthlyCashRegister(string userId)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var result = await _monthlyCashRegisterRepository.DeactivateMonthlyCashRegister(userId);
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

        public async Task<ServiceResponse<bool>> InsertRefundAmount(decimal refundAmount, string userId)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var result = await _monthlyCashRegisterRepository.InsertRefundAmount(refundAmount, userId);
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
    }
}
