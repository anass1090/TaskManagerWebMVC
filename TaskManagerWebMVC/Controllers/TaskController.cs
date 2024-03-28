using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.Models;
using Task = TaskManager.Logic.Models.Task;
using TaskManager.Logic.Managers;
using TaskManagerWebMVC.Models;
using TaskManager.Logic.Interfaces;
using TaskManager.DAL.Repositories;

namespace TaskManager.MVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskService TaskService;
        private readonly TaskRepository TaskRepository;

        public TaskController()
        {
            TaskRepository = new TaskRepository();
            this.TaskService = new TaskService(TaskRepository);
        }

        public IActionResult Index()
        {
            var (tasks, errorMessage) = TaskService.GetAllTasks();

            ViewBag.ErrorMessage = errorMessage;

            List<TaskViewModel> taskViewModels = [];

            foreach (Task task in tasks)
            {
                TaskViewModel taskView = ConvertTaskToTaskView(task);
                taskViewModels.Add(taskView);
            }
            return View(taskViewModels);
        }

        public IActionResult Details(int id)
        {
            var (task, errorMessage) = TaskService.GetTaskById(id);
            ViewBag.ErrorMessage = errorMessage;

            TaskViewModel taskView = new();
            
            if (task != null)
            {
                taskView = ConvertTaskToTaskView(task);
            }

            return View(taskView);
        }

        [HttpPost]
        public IActionResult Create(string title, string description)
        {
            string errorMessage = TaskService.CreateTask(title, description).Item2;
            ViewBag.ErrorMessage = errorMessage;

            if (errorMessage != null)
            {
                ViewData["ErrorMessage"] = errorMessage;
                return View("Create");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Update(int id, string title, string description)
        {

            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

        private static TaskViewModel ConvertTaskToTaskView(Task task)
        {
            TaskViewModel taskView = new()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description
            };

            return taskView;
        }
    }
}
