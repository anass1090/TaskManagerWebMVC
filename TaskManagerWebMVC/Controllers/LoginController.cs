using TaskManager.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using TaskManager.DAL.Repositories;
using TaskManager.Logic.Services;
using System.Web;

namespace TaskManager.MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserService UserService;

        public LoginController()
        {
            UserRepository userRepository = new();
            UserService = new(userRepository);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            (User? user, string? errorMessage) = UserService.AuthenticateUser(email, password);

            if(errorMessage != null)
            {
                ViewBag.ErrorMessage = errorMessage;
                return View();
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}
