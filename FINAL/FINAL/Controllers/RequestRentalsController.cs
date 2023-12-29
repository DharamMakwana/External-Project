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
    public class RequestRentalsController : Controller
    {
        private readonly FinalDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestRentalsController(FinalDBContext context, IHttpContextAccessor httpContextAccessor)
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
            var finalDBContext = _context.RequestRental
                .Include(r => r.Instrument)
                .Include(r => r.Borrower); // Include the BorrowerTb entity

            return View(await finalDBContext.ToListAsync());
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
