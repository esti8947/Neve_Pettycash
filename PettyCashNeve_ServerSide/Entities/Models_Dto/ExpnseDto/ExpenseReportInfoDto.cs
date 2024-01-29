using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models_Dto.ExpenseDto
{
    public class ExpenseReportInfoDto
    {
        public ExpenseDto? Expense { get; set; }
        public string? ExpenseCategoryName { get; set; }
        public string? CategoryName { get; set; }    
        public string? EventName { get; set; }
        public string? BuyerName { get; set; }
    }
}
