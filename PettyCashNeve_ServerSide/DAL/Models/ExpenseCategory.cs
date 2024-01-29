using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class ExpenseCategory
    {
        [Key]
        public int ExpenseCategoryId { get; set; }
        [MaxLength(50)]
        public string expenseCategoryType { get; set; }
        [MaxLength(50)]
        public string ExpenseCategoryName { get; set; }
        [MaxLength (50)]
        public string ExpenseCategoryNameHeb { get; set; }
        [MaxLength (30)]
        public string AccountingCode { get; set; }
        public bool IsActive { get; set; }
    }
}
