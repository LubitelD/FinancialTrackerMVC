using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinancialTrackerDomain;
using FinancialTrackerDomain.Model;

namespace FinancialTrackerInfastructure.Controllers
{
    public class FinancialGoalsController : Controller
    {
        private readonly FinancialTrackerContext _context;

        public FinancialGoalsController(FinancialTrackerContext context)
        {
            _context = context;
        }

        // GET: FinancialGoals
        public async Task<IActionResult> Index()
        {
            var financialTrackerContext = _context.FinancialGoals.Include(f => f.User);
            return View(await financialTrackerContext.ToListAsync());
        }

        // GET: FinancialGoals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var financialGoal = await _context.FinancialGoals
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (financialGoal == null)
            {
                return NotFound();
            }

            return View(financialGoal);
        }

        // GET: FinancialGoals/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "EmailAddress");
            return View();
        }

        // POST: FinancialGoals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TargetAmount,SavedAmount,Deadline,UserId,Id")] FinancialGoal financialGoal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(financialGoal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "EmailAddress", financialGoal.UserId);
            return View(financialGoal);
        }

        // GET: FinancialGoals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var financialGoal = await _context.FinancialGoals.FindAsync(id);
            if (financialGoal == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "EmailAddress", financialGoal.UserId);
            return View(financialGoal);
        }

        // POST: FinancialGoals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TargetAmount,SavedAmount,Deadline,UserId,Id")] FinancialGoal financialGoal)
        {
            if (id != financialGoal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(financialGoal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinancialGoalExists(financialGoal.Id))
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
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "EmailAddress", financialGoal.UserId);
            return View(financialGoal);
        }

        // GET: FinancialGoals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var financialGoal = await _context.FinancialGoals
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (financialGoal == null)
            {
                return NotFound();
            }

            return View(financialGoal);
        }

        // POST: FinancialGoals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var financialGoal = await _context.FinancialGoals.FindAsync(id);
            if (financialGoal != null)
            {
                _context.FinancialGoals.Remove(financialGoal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FinancialGoalExists(int id)
        {
            return _context.FinancialGoals.Any(e => e.Id == id);
        }
    }
}
