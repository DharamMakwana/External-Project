﻿using System;
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
    public class InstrumentsController : Controller
    {
        private readonly FinalDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public InstrumentsController(FinalDbContext context, IHttpContextAccessor httpContextAccessor)
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

        // GET: Instruments
        public async Task<IActionResult> Index()
        {
            // Get the LenderId from the session
            int lenderIdFromSession = HttpContext.Session.GetInt32("LenderId") ?? 0;

            // Filter the data based on the LenderId
            var finalDBContext = _context.Instruments
                .Include(i => i.Lender)
                .Where(i => i.LenderId == lenderIdFromSession);

            return View(await finalDBContext.ToListAsync());
        }

        // GET: Instruments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instruments = await _context.Instruments
                .Include(i => i.Lender)
                .FirstOrDefaultAsync(m => m.InstrumentId == id);
            if (instruments == null)
            {
                return NotFound();
            }

            return View(instruments);
        }

        // GET: Instruments/Create
        public IActionResult Create()
        {
            int lenderIdFromSession = HttpContext.Session.GetInt32("LenderId") ?? 0;

            SelectList lenderList = new SelectList(_context.LenderTb, "LenderId", "LenderId", lenderIdFromSession);

            ViewData["LenderId"] = lenderList;
            return View();
        }

        // POST: Instruments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstrumentId,InstrumentName,LenderId,Description,AvailabilityStatus,RentAmount,Image")] Instruments instruments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instruments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LenderId"] = new SelectList(_context.LenderTb, "LenderId", "LenderId", instruments.LenderId);
            return View(instruments);
        }

        // GET: Instruments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var instruments = await _context.Instruments.FindAsync(id);
            if (instruments == null)
            {
                return NotFound();
            }
            ViewData["LenderId"] = new SelectList(_context.LenderTb, "LenderId", "LenderId", instruments.LenderId);
            return View(instruments);
        }

        // POST: Instruments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstrumentId,InstrumentName,LenderId,Description,AvailabilityStatus,RentAmount,Image")] Instruments instruments)
        {
            if (id != instruments.InstrumentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instruments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstrumentsExists(instruments.InstrumentId))
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
            ViewData["LenderId"] = new SelectList(_context.LenderTb, "LenderId", "LenderId", instruments.LenderId);
            return View(instruments);
        }

        // GET: Instruments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instruments = await _context.Instruments
                .Include(i => i.Lender)
                .FirstOrDefaultAsync(m => m.InstrumentId == id);
            if (instruments == null)
            {
                return NotFound();
            }

            return View(instruments);
        }

        // POST: Instruments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instruments = await _context.Instruments.FindAsync(id);
            _context.Instruments.Remove(instruments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstrumentsExists(int id)
        {
            return _context.Instruments.Any(e => e.InstrumentId == id);
        }
    }
}
