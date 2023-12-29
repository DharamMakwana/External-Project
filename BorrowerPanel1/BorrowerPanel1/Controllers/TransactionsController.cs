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
    public class TransactionsController : Controller
    {
        private readonly FinalDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public TransactionsController(FinalDbContext context, IHttpContextAccessor httpContextAccessor)
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

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            // Retrieve email from session
            string borrowerEmail = HttpContext.Session.GetString("UserId");

            // Check if the email is present in the session
            if (string.IsNullOrEmpty(borrowerEmail))
            {
                // Handle the case where email is not in session
                return RedirectToAction("Login", "Account"); // Redirect to login or handle accordingly
            }

            // Retrieve transactions for the borrower with the specified email
            var transactions = await _context.Transactions
                .Include(t => t.Borrower)
                .Where(t => t.Borrower.EmailId == borrowerEmail)
                .ToListAsync();

            return View(transactions);
        }


        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions
                .Include(t => t.Borrower)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transactions == null)
            {
                return NotFound();
            }

            return View(transactions);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            // Retrieve values from the session
            var emailId = HttpContext.Session.GetString("UserId");
            var reqId = Request.Query["reqId"];
            var amount = Request.Query["amount"];
            var instrumentId = Request.Query["instrumentId"];

            // Find the borrower information from the context using the email ID
            var borrower = _context.BorrowerTb.SingleOrDefault(b => b.EmailId == emailId);

            // Check if the borrower is found
            if (borrower != null)
            {
                // Find Instrument information from the context
                var instrument = _context.Instruments.Find(int.Parse(instrumentId));

                // Check if the instrument is found
                if (instrument != null)
                {
                    // Retrieve LenderId from the instrument
                    var lenderId = instrument.LenderId;

                    // Find RequestRental details associated with ReqId
                    var requestRental = _context.RequestRental
                        .Include(rr => rr.Instrument)
                        .SingleOrDefault(rr => rr.ReqId == int.Parse(reqId));

                    // Check if the RequestRental is found
                    if (requestRental != null)
                    {
                        // Store values in ViewData
                        ViewData["ReqId"] = reqId;
                        ViewData["BorrowerId"] = borrower.BorrowerId;
                        ViewData["BorrowerFName"] = borrower.FirstName;
                        ViewData["BorrowerLName"] = borrower.LastName;
                        ViewData["Amount"] = amount;
                        ViewData["InstrumentId"] = instrumentId;
                        ViewData["LenderId"] = lenderId;
                        ViewData["instrumentName"] = instrument.InstrumentName;
                        ViewData["instrumentDescription"] = instrument.Description;
                        ViewData["instrumentRent"] = instrument.RentAmount;
                        ViewData["instrumentImage"] = instrument.Image;
                        ViewData["instrumentStatus"] = instrument.AvailabilityStatus;

                        // Add RequestRental details to ViewData
                        ViewData["RequestRentalStartDate"] = requestRental.StartDate;
                        ViewData["RequestRentalEndDate"] = requestRental.EndDate;
                        ViewData["RequestRentalExperience"] = requestRental.Experience;
                        ViewData["RequestRentalTotalAmount"] = requestRental.TotalAmount;

                        ViewData["BorrowerIdList"] = new SelectList(_context.BorrowerTb, "BorrowerId", "BorrowerId");

                        return View();
                    }
                    else
                    {
                        // Handle the case where the RequestRental is not found
                        // You might want to redirect or show an error message
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // Handle the case where the instrument is not found
                    // You might want to redirect or show an error message
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // Handle the case where the borrower is not found
                // You might want to redirect or show an error message
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,ReqId,LenderId,BorrowerId,TransactionDate,Amount")] Transactions transactions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transactions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BorrowerId"] = new SelectList(_context.BorrowerTb, "BorrowerId", "BorrowerId", transactions.BorrowerId);

            // Return the view with the model if validation fails
            return View(transactions);
        }


        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions.FindAsync(id);
            if (transactions == null)
            {
                return NotFound();
            }
            ViewData["BorrowerId"] = new SelectList(_context.BorrowerTb, "BorrowerId", "BorrowerId", transactions.BorrowerId);
            return View(transactions);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,ReqId,LenderId,BorrowerId,TransactionDate,Amount")] Transactions transactions)
        {
            if (id != transactions.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transactions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionsExists(transactions.TransactionId))
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
            ViewData["BorrowerId"] = new SelectList(_context.BorrowerTb, "BorrowerId", "BorrowerId", transactions.BorrowerId);
            return View(transactions);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions
                .Include(t => t.Borrower)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transactions == null)
            {
                return NotFound();
            }

            return View(transactions);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transactions = await _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(transactions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionsExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }
    }
}
