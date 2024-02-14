using BL.Services.EventService;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using PettyCashNeve_ServerSide.Dto;
using PettyCashNeve_ServerSide.Repositories.MonthlyCashRegisterRepository;
using PettyCashNeve_ServerSide.Services.MonthlyCashRegisterService;
using System.Security.Claims;

namespace PettyCashNeve_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventController : BaseController
    {
        private readonly IEventService _eventService;
        private readonly IMonthlyCashRegisterService _monthlyCashRegisterService;
        public EventController(IEventService eventService, IMonthlyCashRegisterService monthlyCashRegisterService)
        {
            _eventService = eventService;
            _monthlyCashRegisterService = monthlyCashRegisterService;
        }


        [HttpGet("getEventName/{eventId}")]
        public async Task<IActionResult> GetEventName(int eventId)
        {
            var serviceResponse = await _eventService.GetEventNameById(eventId);
            return HandleResponse(serviceResponse);
        }

        [HttpGet("getEvent/{eventId}")]
        public async Task<IActionResult> GetEvent(int eventId)
        {
            var serviceResponse = await _eventService.GetEventById(eventId);
            return HandleResponse(serviceResponse);
        }

        [HttpGet("getEventsByUserAndMonth")]
        public async Task<IActionResult> GetEventsByUserAndMonth()
        {
            try
            {
                var userId = UserId;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserId is null or empty.");
                }

                var currentMonth = await _monthlyCashRegisterService.GetCurrentMonthByUserIdAsync(userId);

                var serviceResponse = await _eventService.GetEventsByUserAndMonth(userId, currentMonth);

                if (serviceResponse == null)
                {
                    // Handle the case where serviceResponse is null, maybe return a 500 InternalServerError
                    return StatusCode(500, "Failed to retrieve events.");
                }

                return HandleResponse(serviceResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("deleteEvent/{eventId}")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var userId = UserId;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is null or empty.");
            }
            var serviceResponse = await _eventService.DeleteEventById(eventId, userId);
            return HandleResponse(serviceResponse);
        }

        [HttpPut("updateEvent")]
        public async Task<IActionResult> UpdateEvent([FromBody] EventDto updatedEvent)
        {
            var serviceResponse = await _eventService.UpdateEvent(updatedEvent);
            return HandleResponse(serviceResponse);
        }

        [HttpPost("createEvent")]
        public async Task<IActionResult> CreateEvent([FromBody] EventDto newEvent)
        {
            try
            {
                var userId = UserId;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserId is null or empty.");
                }
                newEvent.UpdatedBy = userId;
                newEvent.IsActive = true;

                var serviceResponse = await _eventService.CreateEvent(newEvent);
                return HandleResponse(serviceResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("deactivateAllEvents")]
        public async Task<IActionResult> DeactivateAllEvents()
        {
            var serviceResponse = await _eventService.DeactivateAllEvents();
            return HandleResponse(serviceResponse);
        }
    }
}
