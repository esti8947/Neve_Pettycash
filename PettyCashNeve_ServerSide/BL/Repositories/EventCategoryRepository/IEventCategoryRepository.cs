using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.EventCategoryRepository
{
    public interface IEventCategoryRepository
    {
        Task<List<EventCategory>> GetEventCategoriesAsync();
        Task<EventCategory> GetEventCategoryByIdAsync(int id);
        Task<string> GetEventCategoryNameByIdAsync(int id);
        Task<bool> CreateEventCategoryAsync(EventCategory newEventCategory);
        Task<bool> DeleteEventCategory(int expenseCategoryId);

    }
}
