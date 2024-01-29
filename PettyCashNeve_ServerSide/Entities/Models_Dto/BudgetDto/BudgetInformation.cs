using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models_Dto.BudgetDto
{
    public class BudgetInformation
    {
        public BudgetTypeDto BudgetType { get; set; }
        public AnnualBudgetDto AnnualBudget { get; set; }
        public MonthlyBudgetDto MonthlyBudget { get; set; }
        public RefundBudgetDto RefundBudget { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
