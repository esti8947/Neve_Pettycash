using System.ComponentModel.DataAnnotations;

namespace PettyCashNeve_ServerSide.Dto
{
    public class ExpenseCategoryDto
    {
        public int ExpenseCategoryId { get; set; }
        public string expenseCategoryType { get; set; }
        public string ExpenseCategoryName { get; set; }
        public string ExpenseCategoryNameHeb { get; set; }
        public string AccountingCode { get; set; }
        public bool IsActive { get; set; }
    }
}
