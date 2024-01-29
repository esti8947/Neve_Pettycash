using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class AnnualBudget
    {
        [Key]
        public int AnnualBudgetId { get; set; }
        public int AnnualBudgetYear { get; set; }
        public decimal AnnualBudgetCeiling { get; set; }
        public bool IsActive { get; set; }
        public int DepartmentId { get; set; }

        public Department Department { get; set; }
    }
}
