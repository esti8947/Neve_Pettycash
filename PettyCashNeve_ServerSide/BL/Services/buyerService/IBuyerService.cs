// IBuyerService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Services;
using Entities.Models_Dto;
using PettyCashNeve_ServerSide.Dto;

namespace PettyCashNeve_ServerSide.Services
{
    public interface IBuyerService
    {
        Task<ServiceResponse<List<BuyerDto>>> GetBuyersAsync();
        Task<ServiceResponse<string>> GetBuyerNameById(int buyerId);
        Task<ServiceResponse<bool>> CreateBuyerAsync(BuyerDto buyerDto);
        Task<ServiceResponse<bool>> DeleteBuyerAsync(int buyerId);
    }
}
