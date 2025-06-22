using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinancialTrackerDomain;
using FinancialTrackerDomain.Model;
using NuGet.Protocol.Plugins;
using System.Runtime.InteropServices;
using NuGet.Versioning;

namespace FinancialTrackerInfastructure.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly FinancialTrackerContext _context;

        public TransactionsController(FinancialTrackerContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var financialTrackerContext = _context.Transactions.Include(t => t.Budget).Include(t => t.Category).Include(t => t.FinancialGoal).Include(t => t.User);
            return View(await financialTrackerContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Budget)
                .Include(t => t.Category)
                .Include(t => t.FinancialGoal)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public async Task<IActionResult> Create() 
        {
            ViewData["BudgetId"] = new SelectList(_context.Budgets, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["FinancialGoalId"] = new SelectList(_context.FinancialGoals, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "EmailAddress");
            ViewData["User"] = new SelectList(_context.ApplicationUsers, "Id", "UserName");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Total,CreatedDate,UserId,CategoryId,BudgetId,FinancialGoalId,Id,CategoryType")] Transaction transaction)
        {
            ViewData["BudgetId"] = new SelectList(_context.Budgets, "Id", "Name", transaction.BudgetId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", transaction.CategoryId);
            ViewData["FinancialGoalId"] = new SelectList(_context.FinancialGoals, "Id", "Id", transaction.FinancialGoalId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "EmailAddress", transaction.UserId);
            ViewData["User"] = new SelectList(_context.ApplicationUsers, "Id", "UserName", transaction.User);
            if (ModelState.IsValid)
            {
                using var Transaction = await _context.Database.BeginTransactionAsync();

                try
                {

                    var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == transaction.CategoryId);
                    if (category == null)
                        return StatusCode(500, "Category not found.");

                    var budget = await _context.Budgets.FirstOrDefaultAsync(b => b.Id == transaction.BudgetId);
                    if (budget == null)
                        return StatusCode(500, "Budget not found.");


                    var newTransaction = new Transaction
                    {
                        Total = transaction.Total,
                        User = transaction.User,
                        UserId = transaction.UserId,
                        Category = transaction.Category,
                        CategoryId = transaction.CategoryId,
                        CategoryType = category.CategoryType,
                        BudgetId = transaction.BudgetId,
                        CreatedDate = transaction.CreatedDate,
                        FinancialGoalId = transaction.FinancialGoalId,
                    };


                    _context.Transactions.Add(newTransaction);
                    await _context.SaveChangesAsync();



                    if (category.CategoryType == CategoryType.Expense)
                    {
                        budget.LimitAmount -= transaction.Total;
                        _context.Budgets.Update(budget);
                        await _context.SaveChangesAsync();
                        await Transaction.CommitAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else if (category.CategoryType == CategoryType.Income)
                    {
                        budget.LimitAmount += transaction.Total;
                        _context.Budgets.Update(budget);
                        await _context.SaveChangesAsync();
                        await Transaction.CommitAsync();
                        return RedirectToAction(nameof(Index));
                    };
                    
                }
                catch (Exception ex)
                {
                    await Transaction.RollbackAsync();
                    return StatusCode(500, "Eror saving transaction " + ex.InnerException?.Message ?? ex.Message);
                }
            }
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["BudgetId"] = new SelectList(_context.Budgets, "Id", "Name", transaction.BudgetId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", transaction.CategoryId);
            ViewData["FinancialGoalId"] = new SelectList(_context.FinancialGoals, "Id", "Id", transaction.FinancialGoalId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "EmailAddress", transaction.UserId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Total,CreatedDate,UserId,CategoryId,BudgetId,FinancialGoalId,Id,CategoryType")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BudgetId"] = new SelectList(_context.Budgets, "Id", "Name", transaction.BudgetId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", transaction.CategoryId);
            ViewData["FinancialGoalId"] = new SelectList(_context.FinancialGoals, "Id", "Id", transaction.FinancialGoalId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "EmailAddress", transaction.UserId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Budget)
                .Include(t => t.Category)
                .Include(t => t.FinancialGoal)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
