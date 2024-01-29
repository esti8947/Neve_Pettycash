using System.ComponentModel.DataAnnotations;

namespace PettyCashNeve_ServerSide.Dto
{
    public class EventCategoryDto
    {
        public int EventCategoryId { get; set; }
        public string EventCategoryName { get; set; }
        public string EventCategoryNameHeb { get; set; }
        public bool IsActive { get; set; }
    }
}
