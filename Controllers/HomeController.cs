// Programmer name : S Nondwatyu
// Student nr : 220036624
// Assignment nr : GA1
// Purpose : The purpose  this HomeController is to provide basic functionality
// for rendering views, handling errors, determining user roles,
// and initializing the database in an ASP.NET Core web application

using ASPNETCore_DB.Data;
using ASPNETCore_DB.Interfaces;
using ASPNETCore_DB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASPNETCore_DB.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly ILogger<HomeController> _logger;
        private readonly SQLiteDBContext _context;
        private readonly IDBInitializer _seedDatabase;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, SQLiteDBContext context, IDBInitializer seedDatabase, UserManager<IdentityUser> userManager)
        {
            // Name          : HomeController
            // Purpose       : Initializes a new instance of the HomeController class.
            // Re-use        : None
            // Method Parameters :
            //   ILogger<HomeController> logger
            //     - Logger for logging messages.
            //   SQLiteDBContext context
            //     - Database context for interacting with the SQLite database.
            //   IDBInitializer seedDatabase
            //     - Interface for initializing the database with seed data.
            //   UserManager<IdentityUser> userManager
            //     - Manages users and their roles.
            _logger = logger;
            _context = context;
            _seedDatabase = seedDatabase;
            _userManager = userManager;
        }// end method

        public IActionResult Index()
        {
            // Name          : IActionResult Index()
            // Purpose       : Renders the home page view and sets ViewData with the current user's ID, name, and role.
            // Re-use        : None
            // Method Parameters : None
            // Output Type   : IActionResult
            //   - Returns the view result for the home page.
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            ViewData["UserName"] = _userManager.GetUserName(this.User);

            string userRole = "";
            switch (User.IsInRole("Student"), User.IsInRole("Admin"))
            {
                case (true, false):
                    userRole = "Student";
                    break;
                case (false, true):
                    userRole = "Admin";
                    break;
            }
            
            ViewData["UserRole"] = userRole;

            return View();
        }// end method


        public IActionResult SeedDatabase()
        {
            // Name          : IActionResult SeedDatabase()
            // Purpose       : The purpose of this method is to nitiate the database seeding process and provides feedback to the user.
            // Re-use        : None
            // Method Parameters : None
            // Output Type   : IActionResult
            //   - Returns the view result for database seeding feedback.

            //_seedDatabase.Initialize(_context);
            ViewBag.SeedDbFeedback = "Database created and Student Table populated with Data. Check Database folder.";
            return View("SeedDatabase");
        }// end method

        public IActionResult Privacy()
        {
            // Name          : IActionResult Privacy()
            // Purpose       : The purpose of this method is to render the privacy policy page view.
            // Re-use        : None
            // Method Parameters : None
            // Output Type   : IActionResult
            //   - Returns the view result for the privacy policy page.
            return View();
        }// end method
        public IActionResult About() 
        {
            // Name          : IActionResult About()
            // Purpose       : The purpose of this method is to render the about cut page view.
            // Re-use        : None
            // Method Parameters : None
            // Output Type   : IActionResult
            //   - Returns the view result for the about page.
            return View();
        }// end method
        public IActionResult Life()
        {
            // Name          : IActionResult Life()
            // Purpose       : The purpose of this method is to render the stude life page view.
            // Re-use        : None
            // Method Parameters : None
            // Output Type   : IActionResult
            //   - Returns the view result for the student life page.
            return View();
        }// end method

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Name          : IActionResult Error()
            // Purpose       : Handles errors and renders the error view.
            // Re-use        : None
            // Method Parameters : None
            // Output Type   : IActionResult
            //   - Returns the view result for the error page.
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }// end method
    }//end controller
}//end namespace