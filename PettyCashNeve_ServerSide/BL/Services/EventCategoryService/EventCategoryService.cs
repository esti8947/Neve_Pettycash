using AutoMapper;
using BL.Repositories.EventCategoryRepository;
using DAL.Models;
using PettyCashNeve_ServerSide.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.EventCategoryService
{
    public class EventCategoryService : IEventCategoryService
    {
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IMapper _mapper;
        public EventCategoryService(IEventCategoryRepository eventCategoryRepository, IMapper mapper)
        {
            _eventCategoryRepository = eventCategoryRepository;
            _mapper = mapper;
        }


        public async Task<ServiceResponse<List<EventCategoryDto>>> GetEventCategoriesAsync()
        {
            var serviceResponse = new ServiceResponse<List<EventCategoryDto>>();
            try
            {
                var eventCategories = await _eventCategoryRepository.GetEventCategoriesAsync();
                if(eventCategories != null )
                {
                    var eventCategoriesDtos = _mapper.Map<List<EventCategoryDto>>(eventCategories);
                    serviceResponse.Data = eventCategoriesDtos;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<string>> GetEventCategoryNameById(int eventCategoryId)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var eventCategory = await _eventCategoryRepository.GetEventCategoryByIdAsync(eventCategoryId);

                if (eventCategory != null)
                {
                    serviceResponse.Data = eventCategory.EventCategoryName;
                    serviceResponse.Success = true;
                }
                else
                {
                    serviceResponse.Message = "Event category not found.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> CreateEventCategory(EventCategoryDto eventCategoryDto)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var newEventCategory = _mapper.Map<EventCategory>(eventCategoryDto);
                var result = await _eventCategoryRepository.CreateEventCategoryAsync(newEventCategory);
                serviceResponse.Data = result;
                serviceResponse.Success = result;

                if (!result)
                {
                    serviceResponse.Message = "EventCategory creation failed";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success= false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

    }
}
