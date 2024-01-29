using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Events
    {
        [Key]
        public int EventId { get; set; }
        [MaxLength(150)]
        public string EventName { get; set; }
        public int EventMonth { get; set; }
        public int EventCategoryID { get; set; }
        [MaxLength(128)]
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }

        public EventCategory EventCategory { get; set; }
        public List<Expenses> Expenses { get; set; }
    }
}
