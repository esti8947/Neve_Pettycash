using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class MonthlyBudget
    {
        [Key]
        public int MonthlyBudgetId { get; set; }
        public int MonthlyBudgetYear { get; set; }
        public bool IsActive { get; set; }
        public int DepartmentId { get; set; }
        public int MonthlyBudgetMonth { get; set; }
        public decimal MonthlyBudgetCeiling { get; set; }

        public Department Department { get; set; }
    }
}
