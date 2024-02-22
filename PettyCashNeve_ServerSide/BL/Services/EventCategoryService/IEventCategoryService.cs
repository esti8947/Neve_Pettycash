using PettyCashNeve_ServerSide.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.EventCategoryService
{
    public interface IEventCategoryService
    {
        Task<ServiceResponse<List<EventCategoryDto>>> GetEventCategoriesAsync();
        Task<ServiceResponse<string>> GetEventCategoryNameById(int id);
        Task<ServiceResponse<bool>> CreateEventCategory(EventCategoryDto eventCategoryDto);
        Task<ServiceResponse<bool>> DeleteEventCategory(int eventCategoryId);

    }
}
