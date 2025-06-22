using System.ComponentModel.DataAnnotations;

namespace FinancialTrackerDomain.Model;

public partial class ApplicationUser: Entity
{
    [Display(Name ="Name")]
    public required string UserName { get; set; }

    [Display(Name = "Email")]
    public required string EmailAddress { get; set; }

    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();

    public virtual ICollection<FinancialGoal> FinancialGoals { get; set; } = new List<FinancialGoal>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
