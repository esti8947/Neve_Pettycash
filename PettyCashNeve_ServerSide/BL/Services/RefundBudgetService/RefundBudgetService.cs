using AutoMapper;
using BL.Repositories.RefundBudgetRepository;
using DAL.Models;
using Entities.Models_Dto.BudgetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.RefundBudgetService
{
    public class RefundBudgetService : IRefundBudgetService
    {
        private readonly IRefundBudgetRepository _refundBudgetRepository;
        private readonly IMapper _mapper;

        public RefundBudgetService(IRefundBudgetRepository refundBudgetRepository, IMapper mapper)
        {
            _refundBudgetRepository = refundBudgetRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<RefundBudgetDto>>> GetRefundBudgetsByDepartmentIdAsync(int departmentId)
        {
            var serviceResponse = new ServiceResponse<List<RefundBudgetDto>>();
            try
            {
                var refundBudgets = await _refundBudgetRepository.GetRefundBudgetsByDepartmentIdAsync(departmentId);
                var refundBudgetDtos = _mapper.Map<List<RefundBudgetDto>>(refundBudgets);
                serviceResponse.Data = refundBudgetDtos;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<RefundBudgetDto>> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<RefundBudgetDto>> GetRefundBudgetByDepartmentIdAndIsActiveAsync(int departmentId)
        {
            var serviceResponse = new ServiceResponse<RefundBudgetDto>();
            try
            {
                var refundBudget = await _refundBudgetRepository.GetRefundBudgetByDepartmentIdAndIsActiveAsync(departmentId);
                var refundBudgetDto = _mapper.Map<RefundBudgetDto>(refundBudget);
                serviceResponse.Data = refundBudgetDto;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                return new ServiceResponse<RefundBudgetDto> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<RefundBudgetDto>> CreateRefundBudgetAsync(RefundBudgetDto refundBudgetDto)
        {
            try
            {
                var refundBudget = _mapper.Map<RefundBudget>(refundBudgetDto);
                var createdRefundBudget = await _refundBudgetRepository.CreateRefundBudgetAsync(refundBudget);
                var createdRefundBudgetDto = _mapper.Map<RefundBudgetDto>(createdRefundBudget);
                return new ServiceResponse<RefundBudgetDto> { Data = createdRefundBudgetDto };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<RefundBudgetDto> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<bool>> DeleteRefundBudgetAsync(int refundBudgetId)
        {
            try
            {
                var result = await _refundBudgetRepository.DeleteRefundBudgetAsync(refundBudgetId);
                return new ServiceResponse<bool> { Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, Message = ex.Message };
            }
        }
    }
}
