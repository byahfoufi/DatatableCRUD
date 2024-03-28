using DatatableCRUD.Models;
using Microsoft.AspNetCore.Mvc;

// ... other necessary namespaces 

namespace DatatableCRUD.Controllers
{
    public class SignupController : Controller
    {
        private readonly EmployeeContext _context;

        public SignupController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: /Signup 
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Signup 
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevent Cross-site request forgery (CSRF)
        public async Task<IActionResult> Index(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
            
                // Hash the password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // Create the new employee object
                var newEmployee = new Employee
                {
                    // Map properties from the ViewModel...

                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Username = model.Username,
                    Password = hashedPassword
                };

                // Save to the database
                _context.Employees.Add(newEmployee);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index","Login"); // Redirect to login page
            }

            return View(model); // Return the view with errors
        }
    }
}
