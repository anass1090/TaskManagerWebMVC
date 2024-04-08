using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.Managers;
using TaskManager.DAL.Repositories;
using TaskManagerWebMVC.Models;
using Task = TaskManager.Logic.Models.Task;
using TaskManager.Logic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManager.MVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskService TaskService;

        public TaskController()
        {
            TaskRepository taskRepository = new();
            ProjectRepository projectRepository = new();

            TaskService = new (taskRepository, projectRepository);
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Task> tasks = TaskService.GetAllTasks().Item1;
            string errorMessage = TaskService.GetAllTasks().Item2;

            ViewBag.ErrorMessage = errorMessage;

            List<TaskViewModel> taskViewModels = [];

            foreach (Task task in tasks)
            {
                TaskViewModel taskView = ConvertTaskToTaskView(task);
                taskViewModels.Add(taskView);
            }
            return View(taskViewModels);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Task? task = TaskService.GetTaskById(id).Item1;
            string errorMessage = TaskService.GetTaskById(id).Item2;

            ViewBag.ErrorMessage = errorMessage;

            if (task != null)
            {
                TaskViewModel taskView = ConvertTaskToTaskView(task);
                return View(taskView);
            } else
            {
                return NotFound();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string title, string description, int? projectId)
        {
            string errorMessage = TaskService.CreateTask(title, description, projectId).Item2;
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
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaskViewModel viewModel)
        {
            string errorMessage = TaskService.UpdateTask(viewModel.Id, viewModel.Title, viewModel.Project_Id, viewModel.Description).Item2;

            if (errorMessage == null)
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
            List<Project>? projects = TaskService.GetAllProjects().Item1;

            if (task == null)
            {
                return NotFound();
            }

            TaskViewModel viewModel = ConvertTaskToTaskView(task);

            ViewBag.Projects = projects;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Task? task = TaskService.GetTaskById(id).Item1;

            if (task == null)
            {
                return NotFound();
            }

            TaskViewModel viewModel = ConvertTaskToTaskView(task);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(TaskViewModel viewModel)
        {
            string? errorMessage = TaskService.DeleteTask(viewModel.Id);

            if (errorMessage == null)
            {
                TempData["SuccessMessage"] = "Task deleted successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong deleting the task: " + errorMessage;
                return RedirectToAction("Index");
            }
        }

        private static TaskViewModel ConvertTaskToTaskView(Task task)
        {
            TaskViewModel taskView = new()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Project_Id = task.Project_Id,
            };

            return taskView;
        }
    }
}
