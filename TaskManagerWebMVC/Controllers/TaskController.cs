using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.Managers;
using TaskManager.DAL.Repositories;
using TaskManagerWebMVC.Models;
using Task = TaskManager.Logic.Models.Task;
using TaskManager.Logic.Models;
using TaskManager.Logic.Services;

namespace TaskManager.MVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskService TaskService;
        private readonly ProjectService ProjectService;

        public TaskController(TaskService taskService, ProjectService projectService)
        {
            TaskService = taskService;
            ProjectService = projectService;
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

            if (errorMessage == null)
            {
                TempData["SuccessMessage"] = "Task created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = errorMessage;
                return View("Create");
            }

        }

        [HttpGet]
        public IActionResult Create()
        {
            List<Project> projects = ProjectService.GetAllProjects().Item1;

            ViewBag.Projects = projects;

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
            string errorMessage = TaskService.GetTaskById(id).Item2;

            Task? task = TaskService.GetTaskById(id).Item1;
            List<Project>? projects = ProjectService.GetAllProjects().Item1;
            ViewBag.Projects = projects;

            if (task == null)
            {
                return NotFound();
            }

            TaskViewModel viewModel = ConvertTaskToTaskView(task);

            if (errorMessage != null)
            { 
                ViewBag.ErrorMessage = errorMessage;
            }

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
