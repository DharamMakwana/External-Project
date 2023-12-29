using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LenderPanel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LenderPanel.Controllers
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
        public IActionResult Index()
        {
            // Check if the user is logged in
            if (HttpContext.Session.GetString("UserId") == null)
            {
                // User is not logged in, redirect to the login page
                return RedirectToAction("Login", "Home");
            }

            // Retrieve the lender information based on the user ID stored in the session
            var userId = HttpContext.Session.GetString("UserId");
            var lender = _context.LenderTb.FirstOrDefault(l => l.EmailId == userId);

            if (lender == null)
            {
                // Handle the case where the lender is not found
                return RedirectToAction("NotFound", "Home");
            }

            return View(lender);
        }

        // ... (your existing code)
   

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
            // Check if the email already exists in the database
            if (_context.LenderTb.Any(l => l.EmailId == lenderTb.EmailId))
            {
                ModelState.AddModelError("EmailId", "Email already exists. Please use a different email.");
                return View(lenderTb);
            }

            if (ModelState.IsValid)
            {
                _context.Add(lenderTb);
                await _context.SaveChangesAsync();

                // Redirect to the login page after successful creation
                return RedirectToAction("Login", "Home");
            }

            return View(lenderTb);
        }

        public async Task<IActionResult> ChangePassword(int? id)
        {
            // Check if the user is logged in
            if (HttpContext.Session.GetInt32("LenderId") == null)
            {
                // User is not logged in, redirect to the login page
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var lenderTb = await _context.LenderTb.FindAsync(id);
            if (lenderTb == null)
            {
                return NotFound();
            }

            // Check if the logged-in user has permission to edit this record
            if (lenderTb.EmailId != HttpContext.Session.GetString("UserId"))
            {
                // User does not have permission to edit this record, redirect to a forbidden page or show an error message
                return RedirectToAction("Forbidden", "Home");
            }

            return View(lenderTb);
        }

        // POST: LenderTbs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(int id, [Bind("LenderId,FirstName,LastName,EmailId,ContactNumber,Address,Password")] LenderTb lenderTb)
        {
            // Check if the user is logged in
            if (HttpContext.Session.GetString("UserId") == null)
            {
                // User is not logged in, redirect to the login page
                return RedirectToAction("Login", "Home");
            }

            if (id != lenderTb.LenderId)
            {
                return NotFound();
            }

            // Check if the logged-in user has permission to edit this record
            if (lenderTb.EmailId != HttpContext.Session.GetString("UserId"))
            {
                // User does not have permission to edit this record, redirect to a forbidden page or show an error message
                return RedirectToAction("Forbidden", "Home");
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
        public IActionResult AboutUs()
        {


            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            // Check if the user is logged in
            if (HttpContext.Session.GetInt32("LenderId") == null)
            {
                // User is not logged in, redirect to the login page
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var lenderTb = await _context.LenderTb.FindAsync(id);
            if (lenderTb == null)
            {
                return NotFound();
            }

            // Check if the logged-in user has permission to edit this record
            if (lenderTb.EmailId != HttpContext.Session.GetString("UserId"))
            {
                // User does not have permission to edit this record, redirect to a forbidden page or show an error message
                return RedirectToAction("Forbidden", "Home");
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
            // Check if the user is logged in
            if (HttpContext.Session.GetString("UserId") == null)
            {
                // User is not logged in, redirect to the login page
                return RedirectToAction("Login", "Home");
            }

            if (id != lenderTb.LenderId)
            {
                return NotFound();
            }

            // Check if the logged-in user has permission to edit this record
            if (lenderTb.EmailId != HttpContext.Session.GetString("UserId"))
            {
                // User does not have permission to edit this record, redirect to a forbidden page or show an error message
                return RedirectToAction("Forbidden", "Home");
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
