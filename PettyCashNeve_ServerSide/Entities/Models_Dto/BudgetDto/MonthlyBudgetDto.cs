namespace Entities.Models_Dto.BudgetDto
{
    public class MonthlyBudgetDto
    {
        public int MonthlyBudgetId { get; set; }
        public int MonthlyBudgetYear { get; set; }
        public bool IsActive { get; set; }
        public int DepartmentId { get; set; }
        public int MonthlyBudgetMonth { get; set; }
        public decimal MonthlyBudgetCeiling { get; set; }

    }
}
