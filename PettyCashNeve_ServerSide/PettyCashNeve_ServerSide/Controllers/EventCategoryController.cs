using BL.Services.EventCategoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PettyCashNeve_ServerSide.Dto;

namespace PettyCashNeve_ServerSide.Controllers
{  

    [Route("api/[controller]")]
    [ApiController]
    public class EventCategoryController : BaseController
    {
        private readonly IEventCategoryService _eventCategoryService;
        public EventCategoryController(IEventCategoryService eventCategoryService)
        {
            _eventCategoryService = eventCategoryService;
        }

        [HttpGet("getEventCategoriesAsync")]
        public async Task<IActionResult> GetEventCategoriesAsync()
        {
            var serviceResponse = await _eventCategoryService.GetEventCategoriesAsync();
            return HandleResponse(serviceResponse);
        }

        [HttpGet("getEventCategoryName/{eventCategoryId}")]
        public async Task<IActionResult> GetEventCategoryName(int eventCategoryId)
        {
            var serviceResponse = await _eventCategoryService.GetEventCategoryNameById(eventCategoryId);
            return HandleResponse(serviceResponse);
        }

        [HttpPost("createEventCategory")]
        public async Task<IActionResult> CreateEventCategory([FromBody] EventCategoryDto eventCategoryDto)
        {
            var serviceResponse = await _eventCategoryService.CreateEventCategory(eventCategoryDto);
            return HandleResponse(serviceResponse);
        }
    }
}
