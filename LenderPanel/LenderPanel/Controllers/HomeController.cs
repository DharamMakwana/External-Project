﻿using LenderPanel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LenderPanel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FinalDbContext _context;

      

        public HomeController(ILogger<HomeController> logger, FinalDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var finalDbContext = _context.Instruments.Include(i => i.Lender);
            return View(await finalDbContext.ToListAsync());
        }

        public IActionResult Login(string email, string password)
        {
            // Check if the entered email and password match a record in the LenderTb database
            var lender = _context.LenderTb.FirstOrDefault(l => l.EmailId == email && l.Password == password);

            if (lender != null)
            {
                // Both email and password match, store them in session
                HttpContext.Session.SetString("UserId", lender.EmailId);
                HttpContext.Session.SetString("UserPassword", lender.Password);
                HttpContext.Session.SetInt32("LenderId", lender.LenderId);


                // Redirect to the home page (Index action)
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Email or password is incorrect, show an error message
                ViewBag.ErrorMessage = "Invalid email or password. Please try again.";

                // Return to the login view with an error message
                return View();
            }
        }

        public IActionResult Team()
        {


            return View();
        }
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


        public IActionResult Logout()
        {
            // Remove the session variables
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserPassword");

            // Redirect to the login page (or any other desired page)
            return RedirectToAction("Login");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
