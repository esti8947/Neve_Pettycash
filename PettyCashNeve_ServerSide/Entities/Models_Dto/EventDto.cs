using System.ComponentModel.DataAnnotations;

namespace PettyCashNeve_ServerSide.Dto
{
    public class EventDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public int EventMonth { get; set; }
        public int EventCategoryID { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
