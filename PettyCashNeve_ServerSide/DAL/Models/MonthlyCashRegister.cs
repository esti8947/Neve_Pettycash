using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class MonthlyCashRegister
    {
        [Key]
        public int MonthlyCashRegisterId { get; set; }
        [MaxLength(128)]
        public string UpdatedBy { get; set; }
        [MaxLength(50)]
        public string? MonthlyCashRegisterName { get; set; }
        public int MonthlyCashRegisterMonth { get; set; }
        public int MonthlyCashRegisterYear { get; set; }
        public decimal AmountInCashRegister { get; set; }
        public decimal RefundAmount { get; set; }
        public bool IsActive { get; set; }
    }
}
