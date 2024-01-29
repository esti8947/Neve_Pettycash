using System.ComponentModel.DataAnnotations;

namespace PettyCashNeve_ServerSide.Dto
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string DeptHeadFirstName { get; set; }
        public string DeptHeadLastName { get; set; }
        public string PhonePrefix { get; set; }

        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public bool IsCurrent { get; set; }
        public int CurrentBudgetTypeId { get; set; }
    }
}
