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
    public class RequestRentalsController : Controller
    {
        private readonly FinalDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public RequestRentalsController(FinalDbContext context, IHttpContextAccessor httpContextAccessor)
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

        // GET: RequestRentals
        public async Task<IActionResult> Index()
        {
            // Get the user email from the session
            var userEmail = HttpContext.Session.GetString("UserId");

            // Check if the user email is present in the session
            if (!string.IsNullOrEmpty(userEmail))
            {
                // Retrieve data for the specific user based on the email
                var finalDBContext = _context.RequestRental
                    .Include(r => r.Instrument)
                    .Include(r => r.Borrower)
                    .Where(r => r.Borrower.EmailId == userEmail);


                // Store BorrowerId in ViewData
                var borrower = await _context.BorrowerTb.FirstOrDefaultAsync(b => b.EmailId == userEmail);
                ViewData["BorrowerId"] = borrower?.BorrowerId;

                return View(await finalDBContext.ToListAsync());
            }
            else
            {
                // Handle the case where the user email is not found in the session
                // You might want to redirect to a login page or take appropriate action
                return RedirectToAction("Login", "Account");
            }
        }


        // GET: RequestRentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestRental = await _context.RequestRental
                .Include(r => r.Instrument)
                .FirstOrDefaultAsync(m => m.ReqId == id);
            if (requestRental == null)
            {
                return NotFound();
            }

            return View(requestRental);
        }

        // GET: RequestRentals/Create
        public IActionResult Create()
        {
            // Extract values from query parameters
            var instrumentId = HttpContext.Request.Query["instrumentId"];
            var rentAmount = HttpContext.Request.Query["rentAmount"];
            var sessionId = HttpContext.Session.GetString("UserId");

            var borrower = _context.BorrowerTb.FirstOrDefault(b => b.EmailId == sessionId);

            // Store values in ViewData or ViewBag
            ViewData["InstrumentId"] = instrumentId;
            ViewData["RentAmount"] = rentAmount;
            ViewData["BorrowerId"] = borrower.BorrowerId;



            // Other logic...

            return View();
        }

        // POST: RequestRentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReqId,BorrowerId,InstrumentId,RequestDate,StartDate,EndDate,Experience,Status,TotalAmount")] RequestRental requestRental)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requestRental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstrumentId"] = new SelectList(_context.Instruments, "InstrumentId", "InstrumentId", requestRental.InstrumentId);
            return View(requestRental);
        }

        // GET: RequestRentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestRental = await _context.RequestRental.FindAsync(id);
            if (requestRental == null)
            {
                return NotFound();
            }
            ViewData["InstrumentId"] = new SelectList(_context.Instruments, "InstrumentId", "InstrumentId", requestRental.InstrumentId);
            return View(requestRental);
        }

        // POST: RequestRentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReqId,BorrowerId,InstrumentId,RequestDate,StartDate,EndDate,Experience,Status,TotalAmount")] RequestRental requestRental)
        {
            if (id != requestRental.ReqId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requestRental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestRentalExists(requestRental.ReqId))
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
            ViewData["InstrumentId"] = new SelectList(_context.Instruments, "InstrumentId", "InstrumentId", requestRental.InstrumentId);
            return View(requestRental);
        }

        // GET: RequestRentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestRental = await _context.RequestRental
                .Include(r => r.Instrument)
                .FirstOrDefaultAsync(m => m.ReqId == id);
            if (requestRental == null)
            {
                return NotFound();
            }

            return View(requestRental);
        }

        // POST: RequestRentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requestRental = await _context.RequestRental.FindAsync(id);
            _context.RequestRental.Remove(requestRental);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestRentalExists(int id)
        {
            return _context.RequestRental.Any(e => e.ReqId == id);
        }
    }
}
