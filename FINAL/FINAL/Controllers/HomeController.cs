using FINAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FINAL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FinalDBContext _context;

        public HomeController(ILogger<HomeController> logger, FinalDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get the total count of lenders from the database
            int totalLenderCount = await _context.LenderTb.CountAsync();
            int totalBorrowerCount = await _context.BorrowerTb.CountAsync();
            int totalInstrumentCount = await _context.Instruments.CountAsync();
            int totalRequestCount = await _context.RequestRental.CountAsync();
            int totalTransactions = await _context.Transactions.CountAsync();


            // Send the total count to the view using ViewBag or ViewData
            ViewBag.TotalLenderCount = totalLenderCount;
            ViewBag.TotalBorrowerCount = totalBorrowerCount; // You can also use ViewData["TotalLenderCount"] = totalLenderCount;
            ViewBag.TotalInstrumentCount = totalInstrumentCount; // You can also use ViewData["TotalLenderCount"] = totalLenderCount;
            ViewBag.TotalRequestCount = totalRequestCount;
            ViewBag.TotalTransactions = totalTransactions;

            return View();
        }
        public async Task<IActionResult> Login()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Check if the entered credentials are valid (example: admin@gmail.com and admin123)
            if (email == "admin@gmail.com" && password == "admin123")
            {
                // Set a session variable to indicate that the user is logged in
                HttpContext.Session.SetString("UserId", "admin");

                // Redirect to the home page (Index action)
                return RedirectToAction("Index");
            }
            else
            {
                // Invalid credentials, show an error message
                ViewBag.ErrorMessage = "Invalid credentials. Please try again.";

                // Return to the login view with an error message
                return View();
            }
        }
        public IActionResult Logout()
        {
            // Check if the user is logged in (session variable exists)
            if (HttpContext.Session.GetString("UserId") != null)
            {
                // Remove the user-related session variables
                HttpContext.Session.Remove("UserId");

                // Abandon the session to clear all session data
                HttpContext.Session.Clear();
            }

            // Redirect to the login page
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
