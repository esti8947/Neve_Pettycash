using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models_Dto
{
    public class MonthlyCashRegisterDepartmentDto
    {
        public int DepartmentId { get; set; }
        public int MonthlyCashRegisterMonth { get; set; }
        public int MonthlyCashRegisterYear { get; set; }
        public decimal AmountInCashRegister { get; set; }
        public decimal RefundAmount { get; set; }
    }
}
