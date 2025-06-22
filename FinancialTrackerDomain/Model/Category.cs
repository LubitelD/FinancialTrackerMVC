using System.Text;

namespace FinancialTrackerDomain.Model;


public enum CategoryType
{
 Income = 0,
 Expense = 1
}


public partial class Category : Entity
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public CategoryType? CategoryType { get; set; }
    }
