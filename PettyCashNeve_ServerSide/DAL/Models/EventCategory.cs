using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class EventCategory
    {
        [Key]
        public int EventCategoryId { get; set; }
        [MaxLength(100)]
        public string EventCategoryName { get; set; }
        [MaxLength(100)]
        public string EventCategoryNameHeb { get; set; }
        public bool IsActive { get; set; }
    }
}
