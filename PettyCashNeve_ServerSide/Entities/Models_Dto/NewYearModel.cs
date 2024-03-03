using Entities.Models_Dto.BudgetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models_Dto
{
    public class NewYearModel
    {
        public int NewYear { get; set; }
        public int DepartmentId { get; set; }
        public int BudgetTypeId { get; set; }
        public AnnualBudgetDto AnnualBudget { get; set; }
        public MonthlyBudgetDto MonthlyBudget { get; set; }
    }
}
