using System;
using System.Threading.Tasks;
using AutoMapper;
using BL.Repositories.EventRepository;
using BL.Repositories.ExpenseRepository;
using DAL.Models;
using PettyCashNeve_ServerSide.Dto;

namespace BL.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IExpenseRepository expenseRepository ,IMapper mapper)
        {
            _eventRepository = eventRepository;
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> GetEventNameById(int id)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var eventName = await _eventRepository.GetEventNameById(id);

                if (eventName != null)
                {
                    serviceResponse.Data = eventName;
                    serviceResponse.Success = true;
                }
                else
                {
                    serviceResponse.Message = "Event not found.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<EventDto>> GetEventById(int id)
        {
            var serviceResponse = new ServiceResponse<EventDto>();

            try
            {
                var eventEntity = await _eventRepository.GetEventById(id);

                if (eventEntity != null)
                {
                    var eventDto = _mapper.Map<EventDto>(eventEntity);
                    serviceResponse.Data  = eventDto;
                }
                else
                {
                    serviceResponse.Message = "Event not found.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeleteEventById(int eventId, string userId)
        {
            var serviceResponse = new ServiceResponse<bool>();

            try
            {
                var expensesList = await _expenseRepository.GetUnapprovedExpensesByUserAsync(userId);
                if (expensesList.Any(expense => expense.EventsId == eventId))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Cannot delete event with associated expenses.";
                    return serviceResponse;
                }

                var result = await _eventRepository.DeleteEventById(eventId);
                serviceResponse.Data = result;
                serviceResponse.Success = result;
                if (!result)
                {
                    serviceResponse.Message = "Event not found or deletion failed.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<bool>> CreateEvent(EventDto newEventDto)
        {
            var serviceResponse = new ServiceResponse<bool>();

            try
            {
                var newEvent = _mapper.Map<Events>(newEventDto);
                var result = await _eventRepository.CreateEvent(newEvent);
                serviceResponse.Data = result;
                serviceResponse.Success = result;
                if (!result)
                {
                    serviceResponse.Message = "Event creation failed.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<EventDto>>> GetEventsByUserAndMonth(string updatedBy, int month)
        {
            var serviceResponse = new ServiceResponse<List<EventDto>>();
            try
            {

                var eventsList =  await _eventRepository.GetEventsByUserAndMonth(updatedBy, month);
                var eventsListDto = _mapper.Map<List<EventDto>>(eventsList);
                if(eventsListDto.Count > 0)
                {
                    serviceResponse.Data = eventsListDto;
                    serviceResponse.Success = true;
                }
                else
                {
                    serviceResponse.Message = "no events found";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeactivateAllEvents()
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var result = await _eventRepository.DeactivateAllEvents();
                serviceResponse.Success = result;
                serviceResponse.Data = result;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> UpdateEvent(EventDto eventDto)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var updatedEvent = _mapper.Map<Events>(eventDto);
                var result = await _eventRepository.UpdateEvent(updatedEvent);

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
    }
}
