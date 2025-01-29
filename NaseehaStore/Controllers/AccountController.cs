using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NaseehaStore.Models.Data;
using System.Security.Claims;

namespace NaseehaStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly NaseehaStoreContext _context;

        public AccountController(NaseehaStoreContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.Email == email && a.Password == password);
            if (admin != null)
            {
                // Simple authentication logic
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }  
}