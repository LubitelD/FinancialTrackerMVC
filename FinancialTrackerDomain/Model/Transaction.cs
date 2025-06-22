namespace FinancialTrackerDomain.Model;

public partial class Transaction: Entity
{
    public decimal? Total { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public int UserId { get; set; }

    public required virtual ApplicationUser? User { get; set; }

    public int CategoryId { get; set; }

    public required virtual Category? Category { get; set; }

    public CategoryType? CategoryType { get; set; }

    public int? BudgetId { get; set; }  

    public virtual Budget? Budget { get; set; }

    public int? FinancialGoalId { get; set; }

    public virtual FinancialGoal? FinancialGoal { get; set; }
}
