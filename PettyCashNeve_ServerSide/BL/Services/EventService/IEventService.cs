using DAL.Models;
using PettyCashNeve_ServerSide.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.EventService
{
    public interface IEventService
    {
        Task<ServiceResponse<string>> GetEventNameById(int id);
        Task<ServiceResponse<EventDto>> GetEventById(int id);
        Task<ServiceResponse<bool>> DeleteEventById(int eventId, string userId);
        Task<ServiceResponse<bool>> UpdateEvent(EventDto eventDto);
        Task<ServiceResponse<bool>> CreateEvent(EventDto newEvent);
        Task<ServiceResponse<List<EventDto>>> GetEventsByUserAndMonth(string updatedBy, int month);
        Task<ServiceResponse<bool>> DeactivateAllEvents();
    }
}
