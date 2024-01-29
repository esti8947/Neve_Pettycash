// BuyerController.cs
using Entities.Models_Dto;
using Microsoft.AspNetCore.Mvc;
using PettyCashNeve_ServerSide.Dto;
using PettyCashNeve_ServerSide.Services;

namespace PettyCashNeve_ServerSide.Controllers
{
    public class BuyerController : BaseController
    {
        private readonly IBuyerService _buyerService;

        public BuyerController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }

        [HttpGet("getBuyers")]
        public async Task<IActionResult> GetBuyers()
        {
            var serviceResponse = await _buyerService.GetBuyersAsync();
            return HandleResponse(serviceResponse);
        }

        [HttpGet("getBuyerNameById/{buyerId}")]
        public async Task<IActionResult> GetBuyerNameById(int buyerId)
        {
            var serviceResponse = await _buyerService.GetBuyerNameById(buyerId); 
            return HandleResponse(serviceResponse);
        }

        [HttpPost("createBuyer")]
        public async Task<IActionResult> CreateBuyer([FromBody] BuyerDto buyerDto)
        {
            var serviceResponse = await _buyerService.CreateBuyerAsync(buyerDto);
            return HandleResponse(serviceResponse);
        }

        [HttpDelete("deleteBuyer/{buyerId}")]
        public async Task<IActionResult> DeleteBuyer(int buyerId)
        {
            var serviceResponse = await _buyerService.DeleteBuyerAsync(buyerId);
            return HandleResponse(serviceResponse);
        }
    }
}
