using DatatableCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net; // For password hashing
using Microsoft.EntityFrameworkCore;
// ... other necessary namespaces 

namespace DatatableCRUD.Controllers
{
    public class LoginController : Controller
    {
        private readonly EmployeeContext _context;

        public LoginController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: /Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user 
                var existingUser = await _context.Employees.FirstOrDefaultAsync(e => e.Username == model.Username);

                if (existingUser != null && BCrypt.Net.BCrypt.Verify(model.Password, existingUser.Password))
                {
                    // Successful login (set up session variables etc.)
                    return RedirectToAction("Index", "Employees"); // Adapt to your protected area
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }
            }

            return View(model);
        }
    }
}
