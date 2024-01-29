using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public class Expenses
{
    [Key]
    public int ExpenseId { get; set; }

    public int ExpenseCategoryId { get; set; }
    public int EventsId { get; set; }
    [MaxLength(128)]
    public string UpdatedBy { get; set; }
    [MaxLength(100)]

    public int? DepartmentId {  get; set; }
    public string StoreName { get; set; }
    public DateTime ExpenseDate { get; set; }
    public int RefundMonth { get; set; }

    public bool IsLocked { get; set; }

    public bool IsApproved { get; set; }

    public int? BuyerId { get; set; }

    public bool IsActive { get; set; }
    [MaxLength(250)]
    public string? InvoiceScan { get; set; }

    public decimal ExpenseAmount { get; set; }
    [MaxLength(200)]
    public string? Notes { get; set; }

    public Buyer Buyer { get; set; }

    public ExpenseCategory ExpenseCategory { get; set; }
    public Events Events { get; set; }
    public Department Department { get; set; }

    //public virtual ICollection<ExpenseOfEvent> ExpenseOfEvents { get; set; } = new List<ExpenseOfEvent>();

}
