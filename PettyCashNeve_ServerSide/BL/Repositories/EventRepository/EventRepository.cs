using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BL.Repositories.EventRepository
{
    public class EventRepository : IEventRepository
    {
        private readonly PettyCashNeveDbContext _context;

        public EventRepository(PettyCashNeveDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetEventNameById(int id)
        {
            try
            {
                var eventName = await _context.Events
                    .Where(e => e.EventId == id)
                    .Select(e => e.EventName)
                    .FirstOrDefaultAsync();

                return eventName;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<Events> GetEventById(int id)
        {
            try
            {
                var eventEntity = await _context.Events
                    .FirstOrDefaultAsync(e => e.EventId == id && e.IsActive);

                return eventEntity;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<List<Events>> GetEventsByUserAndMonth(string userId, int month)
        {
            try
            {
                var eventsList = await _context.Events.Where(e => e.UpdatedBy.Equals(userId) && e.EventMonth.Equals(month) && e.IsActive).ToListAsync();
                return eventsList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> DeleteEventById(int id)
        {
            try
            {
                var eventEntity = await _context.Events.FindAsync(id);

                if (eventEntity == null)
                {
                    // Handle the case where the entity to delete is not found
                    return false;
                }

                eventEntity.IsActive = false;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<bool> CreateEvent(Events newEvent)
        {
            try
            {
                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<bool> DeactivateAllEvents()
        {
            try
            {
                var allEvents = await _context.Events.ToListAsync();

                foreach (var evt in allEvents)
                {
                    evt.IsActive = false;
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                return false;
            }
        }

        public async Task<bool> UpdateEvent(Events updatedEvent)
        {
            try
            {
                var eventToUpdata = await GetEventById(updatedEvent.EventId);
                if(eventToUpdata == null)
                {
                    throw new DirectoryNotFoundException("event to fount or not active");
                }
                _context.Entry(eventToUpdata).CurrentValues.SetValues(updatedEvent);
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
