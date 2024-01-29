using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        [MaxLength(20)]
        public string DepartmentCode { get; set; }
        [MaxLength(50)]
        public string DepartmentName { get; set; }
        [MaxLength(20)]
        public string DeptHeadFirstName { get; set; }
        [MaxLength(20)]
        public string DeptHeadLastName { get; set; }
        [MaxLength(3)] 
        public string? PhonePrefix { get; set; }

        [MaxLength(7)] 
        public string? PhoneNumber { get; set; }
        [MaxLength(50)]
        public string? Description { get; set; }
        public bool IsCurrent { get; set; }
        public int CurrentBudgetTypeId { get; set; }

        public BudgetType CurrentBudgetType { get; set; }
        public virtual ICollection<AnnualBudget> AnnualBudgets { get; set; } = new List<AnnualBudget>();
        public virtual ICollection<MonthlyBudget> MonthlyBudgets { get; set; } = new List<MonthlyBudget>();
        public virtual ICollection<RefundBudget> RefundBudgets { get; set; } = new List<RefundBudget>();
    }
}
