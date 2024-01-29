using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class RefundBudget
    {
        [Key]
        public int RefundBudgetId { get; set; }
        public int DepartmentId { get; set; }
        public int RefundBudgetYear { get; set; }
        public bool IsActive { get; set; }

        public Department Department { get; set; }
    }
}
