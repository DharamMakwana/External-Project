using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FINAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FINAL.Controllers
{
    public class BorrowerTbsController : Controller
    {
        private readonly FinalDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public BorrowerTbsController(FinalDBContext context, IHttpContextAccessor httpContextAccessor)
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
        // GET: BorrowerTbs
        public async Task<IActionResult> Index()
        {
            return View(await _context.BorrowerTb.ToListAsync());
        }

        // GET: BorrowerTbs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowerTb = await _context.BorrowerTb
                .FirstOrDefaultAsync(m => m.BorrowerId == id);
            if (borrowerTb == null)
            {
                return NotFound();
            }

            return View(borrowerTb);
        }

        // GET: BorrowerTbs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BorrowerTbs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BorrowerId,FirstName,LastName,EmailId,ContactNumber,Address")] BorrowerTb borrowerTb)
        {
            if (ModelState.IsValid)
            {
                _context.Add(borrowerTb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(borrowerTb);
        }

        // GET: BorrowerTbs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowerTb = await _context.BorrowerTb.FindAsync(id);
            if (borrowerTb == null)
            {
                return NotFound();
            }
            return View(borrowerTb);
        }

        // POST: BorrowerTbs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BorrowerId,FirstName,LastName,EmailId,ContactNumber,Address")] BorrowerTb borrowerTb)
        {
            if (id != borrowerTb.BorrowerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrowerTb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowerTbExists(borrowerTb.BorrowerId))
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
            return View(borrowerTb);
        }

        // GET: BorrowerTbs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowerTb = await _context.BorrowerTb
                .FirstOrDefaultAsync(m => m.BorrowerId == id);
            if (borrowerTb == null)
            {
                return NotFound();
            }

            return View(borrowerTb);
        }

        // POST: BorrowerTbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var borrowerTb = await _context.BorrowerTb.FindAsync(id);
            _context.BorrowerTb.Remove(borrowerTb);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowerTbExists(int id)
        {
            return _context.BorrowerTb.Any(e => e.BorrowerId == id);
        }
    }
}
