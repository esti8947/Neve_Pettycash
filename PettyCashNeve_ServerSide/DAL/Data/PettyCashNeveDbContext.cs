using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class PettyCashNeveDbContext : IdentityDbContext<NdbUser>
    {
        public PettyCashNeveDbContext(DbContextOptions<PettyCashNeveDbContext> options)
            : base(options)
        {
        }

        public DbSet<AnnualBudget> AnnualBudgets { get; set; }
        public DbSet<BudgetType> BudgetTypes { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<MonthlyBudget> MonthlyBudgets { get; set; }
        public DbSet<MonthlyCashRegister> MonthlyCashRegisters { get; set; }
        public DbSet<RefundBudget> RefundBudgets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships and constraints here

            builder.Entity<Events>()
                .HasMany(e => e.Expenses)
                .WithOne(expense => expense.Events)
                .HasForeignKey(expense => expense.EventsId);

            builder.Entity<Department>()
                .HasOne(d => d.CurrentBudgetType)
                .WithMany(bt => bt.Departments)
                .HasForeignKey(d => d.CurrentBudgetTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add other configurations as needed

            // Adding non-clustered index to Expenses entity
            builder.Entity<Expenses>()
                .HasIndex(e => new { e.UpdatedBy, e.ExpenseDate, e.RefundMonth, e.IsApproved, e.IsActive })
                .IsUnique(false)
                .HasDatabaseName("IX_Expenses_FilterIndex");
        }
    }
}
