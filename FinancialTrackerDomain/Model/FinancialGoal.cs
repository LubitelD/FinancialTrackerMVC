namespace FinancialTrackerDomain.Model;

public partial class FinancialGoal: Entity
{
    public decimal? TargetAmount { get; set; }

    public decimal? SavedAmount { get; set; }

    public DateOnly? Deadline { get; set; }

    public int UserId { get; set; }

    public virtual required ApplicationUser? User { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
