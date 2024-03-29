using Microsoft.AspNetCore.Mvc;
using Task = TaskManager.Logic.Models.Task;
using TaskManager.Logic.Managers;
using TaskManager.DAL.Repositories;
using TaskManagerWebMVC.Models;

namespace TaskManager.MVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskService TaskService;

        public TaskController()
        {
            TaskRepository taskRepository = new();
            TaskService = new TaskService(taskRepository);
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

            if (errorMessage == null)
            {
                TempData["SuccessMessage"] = "Task created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["ErrorMessage"] = errorMessage;
                return View("Create");
            }

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(TaskViewModel viewModel)
        {
            string errorMessage = TaskService.UpdateTask(viewModel.Id, viewModel.Title, viewModel.Description).Item2;

            if(errorMessage == null)
            {
                TempData["SuccessMessage"] = "Task updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = errorMessage;
                return View("Edit");
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Task? task = TaskService.GetTaskById(id).Item1;

            if (task == null)
            {
                return NotFound();
            }

            TaskViewModel viewModel = ConvertTaskToTaskView(task);

            return View(viewModel);
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
