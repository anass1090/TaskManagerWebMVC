using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.Models;
using TaskManager.DAL.Repositories;
using Task = TaskManager.Logic.Models.Task;
using TaskManager.Logic.Managers;
using TaskManagerWebMVC.Models;

namespace TaskManager.MVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskService TaskService;

        public TaskController() 
        {
            TaskService = new TaskService();
        }

        public IActionResult Index()
        {
            List<Task> tasks = TaskService.GetAllTasks();
            List<TaskViewModel> taskViewModels = [];

            foreach (Task task in tasks)
            {
                TaskViewModel taskView = new()
                {
                    Title = task.Title,
                    Description = task.Description
                };

                taskViewModels.Add(taskView);
            }
            return View(taskViewModels);
        }

        public IActionResult View(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string title, string description)
        {
            TaskService.CreateTask(title, description);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Update()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }
    }
}
