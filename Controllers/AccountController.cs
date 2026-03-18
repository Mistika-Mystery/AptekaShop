using apteka.Data;
using apteka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomIdentityApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext2 _context;

        public AccountController(ApplicationDbContext2 context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.IdRole = 2;
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Login));
            }

            return View(user);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string login, string password)
        {
            var user = _context.Users
                .Include(item => item.Role)
                .FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.IdUser);
                HttpContext.Session.SetString("UserRole", ResolveRoleName(user));
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private static string ResolveRoleName(User user)
        {
            if (!string.IsNullOrWhiteSpace(user.Role?.name))
            {
                return user.Role.name.Trim().ToLowerInvariant();
            }

            return user.IdRole == 1 ? "admin" : "user";
        }
    }
}
