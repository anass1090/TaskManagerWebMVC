using Microsoft.AspNetCore.Mvc;
using TaskManager.DAL.Repositories;
using TaskManager.Logic.Services;
using TaskManager.Logic.Models;

namespace TaskManager.MVC.Controllers
{
    public class RegisterController(UserService userService) : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string firstName, string lastName, string email, string password)
        {
            (User? user, string? errorMessage) = userService.CreateUser(firstName, lastName, email, password);
            
            if(errorMessage != null)
            {
                ViewBag.ErrorMessage = errorMessage;
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
