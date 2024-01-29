// BuyerService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BL.Repositories.BuyerRepository;
using BL.Services;
using DAL.Models;
using Entities.Models_Dto;
using PettyCashNeve_ServerSide.Dto;
using PettyCashNeve_ServerSide.Repositories;

namespace PettyCashNeve_ServerSide.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly IBuyerRepository _buyerRepository;
        private readonly IMapper _mapper;

        public BuyerService(IBuyerRepository buyerRepository, IMapper mapper)
        {
            _buyerRepository = buyerRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<BuyerDto>>> GetBuyersAsync()
        {
            var serviceResponse = new ServiceResponse<List<BuyerDto>>();
            try
            {
                var buyers = await _buyerRepository.GetBuyersAsync();
                var buyerDtos = _mapper.Map<List<BuyerDto>>(buyers);
                serviceResponse.Data = buyerDtos;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<BuyerDto>> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<bool>> CreateBuyerAsync(BuyerDto buyerDto)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var buyer = _mapper.Map<Buyer>(buyerDto);
                var success = await _buyerRepository.CreateBuyerAsync(buyer);
                serviceResponse.Data = success;
                serviceResponse.Success = success;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeleteBuyerAsync(int buyerId)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var result = await _buyerRepository.DeleteBuyerAsync(buyerId);
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

        public async Task<ServiceResponse<string>> GetBuyerNameById(int buyerId)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                var buyer = await _buyerRepository.GetBuyeByIdrAsync(buyerId);
                if(buyer != null)
                {
                    serviceResponse.Data = buyer.BuyerName;
                    serviceResponse.Success = true;
                }
                else
                {
                    serviceResponse.Message = "Buyer not found";
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
