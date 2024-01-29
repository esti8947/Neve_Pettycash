using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models_Dto.ExpenseDto
{
    public class ExpenseDto
    {
        public int ExpenseId { get; set; }

        public int ExpenseCategoryId { get; set; }
        public int EventsId { get; set; }
        public int DepartmentId { get; set; }
        public string UpdatedBy { get; set; }
        public string StoreName { get; set; }
        public DateTime ExpenseDate { get; set; }
        public int RefundMonth { get; set; }

        public bool IsLocked { get; set; }

        public bool IsApproved { get; set; }

        public int? BuyerId { get; set; }

        public bool IsActive { get; set; }
        public string InvoiceScan { get; set; }

        public decimal ExpenseAmount { get; set; }
        public string Notes { get; set; }
    }
}
