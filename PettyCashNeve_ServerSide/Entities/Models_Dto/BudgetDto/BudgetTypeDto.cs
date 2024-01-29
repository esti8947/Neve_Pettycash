using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models_Dto.BudgetDto
{
    public class BudgetTypeDto
    {
        public int BudgetTypeId { get; set; }
        public string BudgetTypeName { get; set; }
        public string budgetTypeNameHeb { get; set; }
        public string BudgetTypeDescription { get; set; } = string.Empty;
    }
}
