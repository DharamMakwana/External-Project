using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BorrowerPanel1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BorrowerPanel1.Controllers
{
    public class LenderTbsController : Controller
    {
        private readonly FinalDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LenderTbsController(FinalDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Check if the session contains the user ID
            if (_httpContextAccessor.HttpContext.Session.GetString("UserId") == null)
            {
                // If not, redirect to the login page
                context.Result = RedirectToAction("Login", "Home");
            }

            base.OnActionExecuting(context);
        }

        // GET: LenderTbs
        public async Task<IActionResult> Index()
        {
            return View(await _context.LenderTb.ToListAsync());
        }

        // GET: LenderTbs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lenderTb = await _context.LenderTb
                .FirstOrDefaultAsync(m => m.LenderId == id);
            if (lenderTb == null)
            {
                return NotFound();
            }

            return View(lenderTb);
        }

        // GET: LenderTbs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LenderTbs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LenderId,FirstName,LastName,EmailId,ContactNumber,Address,Password")] LenderTb lenderTb)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lenderTb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lenderTb);
        }

        // GET: LenderTbs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lenderTb = await _context.LenderTb.FindAsync(id);
            if (lenderTb == null)
            {
                return NotFound();
            }
            return View(lenderTb);
        }

        // POST: LenderTbs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LenderId,FirstName,LastName,EmailId,ContactNumber,Address,Password")] LenderTb lenderTb)
        {
            if (id != lenderTb.LenderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lenderTb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LenderTbExists(lenderTb.LenderId))
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
            return View(lenderTb);
        }

        // GET: LenderTbs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lenderTb = await _context.LenderTb
                .FirstOrDefaultAsync(m => m.LenderId == id);
            if (lenderTb == null)
            {
                return NotFound();
            }

            return View(lenderTb);
        }

        // POST: LenderTbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lenderTb = await _context.LenderTb.FindAsync(id);
            _context.LenderTb.Remove(lenderTb);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LenderTbExists(int id)
        {
            return _context.LenderTb.Any(e => e.LenderId == id);
        }
    }
}
