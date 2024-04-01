using BL.Services.EventCategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PettyCashNeve_ServerSide.Dto;

namespace PettyCashNeve_ServerSide.Controllers
{  

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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


        [HttpGet("getAllEventCategories")]
        public async Task<IActionResult> GetAllEventCategories()
        {
            var serviceResponse = await _eventCategoryService.GetAllEventCategories();
            return HandleResponse(serviceResponse);
        }

        [HttpGet("getEventCategoryName/{eventCategoryId}")]
        public async Task<IActionResult> GetEventCategoryName(int eventCategoryId)
        {
            var serviceResponse = await _eventCategoryService.GetEventCategoryNameById(eventCategoryId);
            return HandleResponse(serviceResponse);
        }

        [HttpPost("createEventCategory")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEventCategory([FromBody] EventCategoryDto eventCategoryDto)
        {
            var serviceResponse = await _eventCategoryService.CreateEventCategory(eventCategoryDto);
            return HandleResponse(serviceResponse);
        }

        [HttpDelete("deleteEventCategory/{eventCategoryId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEventCategory(int eventCategoryId)
        {
            var serviceResponse = await _eventCategoryService.DeleteEventCategory(eventCategoryId);
            return HandleResponse(serviceResponse);
        }

        [HttpGet("activateEventCategory/{eventCategoryId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeEventCategoryStatusAsync(int eventCategoryId)
        {
            var serviceResponse = await _eventCategoryService.ActivateEventCategory(eventCategoryId);
            return HandleResponse(serviceResponse);
        }
        
    }
}
