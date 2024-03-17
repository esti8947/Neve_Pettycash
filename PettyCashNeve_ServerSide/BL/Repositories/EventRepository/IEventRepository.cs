using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace BL.Repositories.EventRepository
{
    public interface IEventRepository
    {
        Task<string> GetEventNameById(int id);
        Task<Events> GetEventById(int id);
        Task<List<Events>> GetEventsByDepartmentId(int departmentId);
        Task<bool> DeleteEventById(int id);
        Task<bool> UpdateEvent(Events updatedEvent);
        Task<bool> CreateEvent(Events newEvent);
        Task<List<Events>> GetEventsByUserAndMonth(string updatedBy, int month);
        Task<bool> DeactivateAllEvents();
    }
}
