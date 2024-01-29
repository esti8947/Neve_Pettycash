using System.ComponentModel.DataAnnotations;

namespace PettyCashNeve_ServerSide.Dto
{
    public class MonthlyCashRegisterDto
    {
        public int MonthlyCashRegisterId { get; set; }
        public string UpdatedBy { get; set; }
        public string MonthlyCashRegisterName { get; set; }
        public int MonthlyCashRegisterMonth { get; set; }
        public int MonthlyCashRegisterYear { get; set; }
        public decimal AmountInCashRegister { get; set; }
        public decimal RefundAmount { get; set; }
        public bool IsActive { get; set; }
    }
}
