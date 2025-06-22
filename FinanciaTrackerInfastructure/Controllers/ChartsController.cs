using FinancialTrackerDomain;
using FinancialTrackerDomain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialTrackerInfastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private record CountByCategorytype(string CategoryType, int Count);



        private readonly FinancialTrackerContext financialTrackerContext;

        public ChartsController(FinancialTrackerContext financialTrackerContext)
        {
            this.financialTrackerContext = financialTrackerContext;
        }

        [HttpGet("countByCategoryType")]
        public async Task<JsonResult> GetCountByCategoryTypeAsync(CancellationToken cancellationToken)
        {
            var responseItems = await financialTrackerContext.
                Transactions.GroupBy(CategoryType => CategoryType.CategoryType).
                Select(group => new CountByCategorytype(group.Key.ToString(), group.Count())).
                ToListAsync(cancellationToken);
            return new JsonResult(responseItems);
        }

        [HttpGet("budgetGraph")]
        public async Task<JsonResult> GetBudgetGraphAsync(CancellationToken cancellationToken)
        {
            var responseItems = await financialTrackerContext.Transactions
                .GroupBy(t => t.CreatedDate)
                .Select(group => new
                {
                    date = group.Key.Value.ToString("yyyy-MM-dd"),
                    total = group.Sum(t => t.CategoryType == CategoryType.Expense ? -t.Total : t.Total)
                })
                .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }
    } 
}
