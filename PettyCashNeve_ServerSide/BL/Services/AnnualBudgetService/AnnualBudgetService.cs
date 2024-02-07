using AutoMapper;
using BL.Repositories.AnnualBudgetRepository;
using DAL.Models;
using Entities.Models_Dto.BudgetDto;
using PettyCashNeve_ServerSide.Repositories.DepartmentRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.AnnualBudgetService
{
    public class AnnualBudgetService : IAnnualBudgetService
    {
        private readonly IAnnualBudgetRepository _annualBudgetRepository;
        private readonly IMapper _mapper;

        public AnnualBudgetService(IAnnualBudgetRepository annualBudgetRepository, IMapper mapper)
        {
            _annualBudgetRepository = annualBudgetRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<AnnualBudgetDto>>> GetAnnualBudgetsByDepartmentIdAsync(int departmentId)
        {
            var serviceResponse = new ServiceResponse<List<AnnualBudgetDto>>();
            try
            {
                var annualBudgets = await _annualBudgetRepository.GetAnnualBudgetsByDepartmentIdAsync(departmentId);
                var annualBudgetDtos = _mapper.Map<List<AnnualBudgetDto>>(annualBudgets);
                serviceResponse.Data = annualBudgetDtos;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<AnnualBudgetDto>> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<AnnualBudgetDto>> GetAnnualBudgetByDepartmentIdAndIsActiveAsync(int departmentId)
        {
            var serviceResponse = new ServiceResponse<AnnualBudgetDto>();
            try
            {
                var annualBudget = await _annualBudgetRepository.GetAnnualBudgetsByDepartmentIdAndIsActiveAsync(departmentId);
                var annualBudgetDto = _mapper.Map<AnnualBudgetDto>(annualBudget);
                serviceResponse.Data = annualBudgetDto;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AnnualBudgetDto> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<AnnualBudgetDto>> CreateAnnualBudgetAsync(AnnualBudgetDto annualBudgetDto)
        {
            try
            {
                var annualBudget = _mapper.Map<AnnualBudget>(annualBudgetDto);
                var createdAnnualBudget = await _annualBudgetRepository.CreateAnnualBudgetAsync(annualBudget);
                var createdAnnualBudgetDto = _mapper.Map<AnnualBudgetDto>(createdAnnualBudget);
                return new ServiceResponse<AnnualBudgetDto> { Data = createdAnnualBudgetDto };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AnnualBudgetDto> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<bool>> AddSumToAnnualBudgetCeilingAsync(int annualBudgetId, int additionalAmount)
        {
            try
            {
                var result = await _annualBudgetRepository.AddSumToAnnualBudgetCeilingAsync(annualBudgetId, additionalAmount);
                return new ServiceResponse<bool> { Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<bool>> DeleteAnnualBudgetAsync(int annualBudgetId)
        {
            try
            {
                var result = await _annualBudgetRepository.DeleteAnnualBudgetAsync(annualBudgetId);
                return new ServiceResponse<bool> { Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<bool>> resettingAnnualBudget(int departmentId)
        {
            try
            {
                var result = await _annualBudgetRepository.resettingAnnualBudget(departmentId);
                return new ServiceResponse<bool> { Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<bool>> addAmountToAnnualBudget(int departmentId, int amountToAdd)
        {
            try
            {
                var result = await _annualBudgetRepository.addAmountToAnnualBudget(departmentId, amountToAdd);
                return new ServiceResponse<bool> { Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, Message = ex.Message };
            }
        }
    }
}
