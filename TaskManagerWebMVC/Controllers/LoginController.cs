using TaskManager.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.Services;

namespace TaskManager.MVC.Controllers
{
    public class LoginController(UserService userService) : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            (User? user, string? errorMessage) = userService.AuthenticateUser(email, password);

            if(user == null)
            {
                ViewBag.ErrorMessage = errorMessage;
                return View();
            }

            HttpContext.Session.SetInt32("userId", user.Id);

            return RedirectToAction("Index", "Home");
        }
    }
}
