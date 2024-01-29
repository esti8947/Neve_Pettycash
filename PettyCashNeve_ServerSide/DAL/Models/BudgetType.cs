using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class BudgetType
    {
        [Key]
        public int BudgetTypeId { get; set; }
        [MaxLength(50)]
        public string BudgetTypeName { get; set; }
        [MaxLength(50)]
        public string budgetTypeNameHeb { get; set; }
        [MinLength(200)]
        public string? BudgetTypeDescription { get; set; }

        public List<Department> Departments { get; set; }
    }
}
