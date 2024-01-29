namespace Entities.Models_Dto.BudgetDto
{
    public class AnnualBudgetDto
    {
        public int AnnualBudgetId { get; set; }
        public int AnnualBudgetYear { get; set; }
        public decimal AnnualBudgetCeiling { get; set; }
        public bool IsActive { get; set; }
        public int DepartmentId { get; set; }
    }
}
