using apteka.Data;
using apteka.Models;
using CustomIdentityApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CustomIdentityApp.Controllers
{
    public class AccountController : Controller
    {

            private readonly ApplicationDbContext2 _context;

            public AccountController(ApplicationDbContext2 context)
            {
                _context = context;
            }

     [HttpGet]
     public IActionResult Register()
            {
                return View();
            }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.IdRole = 2;  // Назначение роли 2
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user != null)
            {
                // Здесь можно использовать куки или сессии для авторизации
                HttpContext.Session.SetInt32("UserId", user.IdUser);
                return RedirectToAction("Index", "Leks");
            }

            ModelState.AddModelError("", "Неверный логин или пароль.");
            return View();
        }


    }
}
