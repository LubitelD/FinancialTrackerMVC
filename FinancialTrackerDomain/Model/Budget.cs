using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinancialTrackerDomain.Model;

public partial class Budget: Entity
{
    public required string Name { get; set; }

    public int UserId { get; set; }

    public required virtual ApplicationUser? User { get; set; }

    [DataType(DataType.Currency)]
    public decimal? LimitAmount { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
