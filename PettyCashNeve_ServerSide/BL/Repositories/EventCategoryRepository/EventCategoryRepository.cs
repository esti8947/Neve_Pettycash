using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using PettyCashNeve_ServerSide.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.EventCategoryRepository
{
    public class EventCategoryRepository : IEventCategoryRepository
    {
        private readonly PettyCashNeveDbContext _context;

        public EventCategoryRepository(PettyCashNeveDbContext context)
        {
            _context = context;
        }

        public async Task<List<EventCategory>> GetEventCategoriesAsync()
        {
            var eventCategoriesList = await _context.EventCategories.ToListAsync();
            return eventCategoriesList;
        }

        public async Task<EventCategory> GetEventCategoryByIdAsync(int eventCategoryId)
        {
            try
            {
                var eventCategory = await _context.EventCategories
                    .FirstOrDefaultAsync(ec => ec.EventCategoryId == eventCategoryId && ec.IsActive == true);

                if (eventCategory == null)
                {
                    throw new NotFoundException("Event category not found or not active.");
                }

                return eventCategory;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<string> GetEventCategoryNameByIdAsync(int id)
        {
            try
            {
                var eventCategoryName = await _context.EventCategories
                    .Where(ec => ec.EventCategoryId == id)
                    .Select(ec => ec.EventCategoryName)
                    .FirstOrDefaultAsync();

                return eventCategoryName;
            }
            catch (Exception ex)
            {
                // Handle exception (log, throw, etc.)
                throw;
            }
        }

        public async Task<bool> CreateEventCategoryAsync(EventCategory newEventCategory)
        {
            try
            {
                _context.EventCategories.Add(newEventCategory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
