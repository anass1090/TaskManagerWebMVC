using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.Managers;
using TaskManagerWebMVC.Models;
using Task = TaskManager.Logic.Models.Task;
using TaskManager.Logic.Models;
using TaskManager.Logic.Services;
using TaskManager.Logic.Exceptions;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace TaskManager.MVC.Controllers
{
    public class TaskController(TaskService taskService, ProjectService projectService) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("userId") == null)
            {
                TempData["errorMessage"] = "something went wrong try logging in again.";
                return RedirectToAction(nameof(Login), nameof(Login));
            }
            else
            {
                (List<Task>? tasks, string? errorMessage) = taskService.GetAllTasks(HttpContext.Session.GetInt32("userId").Value);

                ViewBag.ErrorMessage = errorMessage;
                List<TaskViewModel> taskViewModels = [];

                if (tasks != null)
                {
                    foreach (Task task in tasks)
                    {
                        TaskViewModel taskView = ConvertTaskToTaskView(task);
                        taskViewModels.Add(taskView);
                    }
                }
                return View(taskViewModels);
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
                Task task = taskService.GetTaskById(id, HttpContext.Session.GetInt32("userId"));

                return View(ConvertTaskToTaskView(task));
            }
            catch (TaskException ex)
            {
                TempData["ErrorMessage"] = "Error fetching task: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (DatabaseException ex)
            {
                TempData["ErrorMessage"] = "Error fetching task: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (UserException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Login), nameof(Login));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error fetching task, try again later.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskViewModel model)
        {
            ViewBag.Projects = projectService.GetAllProjects().Item1;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                taskService.CreateTask(model.Title, model.Description, model.Project_Id, HttpContext.Session.GetInt32("userId").Value);

                TempData["SuccessMessage"] = "Task created successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (TaskException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(model);
            }
            catch (DatabaseException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Error while creating task, try again later.";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("userId") == null)
            {
                TempData["errorMessage"] = "something went wrong try logging in again.";
                return RedirectToAction(nameof(Login), nameof(Login));
            }

            (ViewBag.Projects, _) = projectService.GetAllProjects();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaskViewModel viewModel)
        {
            string? errorMessage = taskService.UpdateTask(viewModel.Id, viewModel.Title, viewModel.Project_Id, viewModel.Description, HttpContext.Session.GetInt32("userId")).Item2;

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
            int? userId = HttpContext.Session.GetInt32("userId");
            Task task = taskService.GetTaskById(id, userId);

            if (task == null)
            {
                return RedirectToAction("Index", "Home");
            }

            TaskViewModel viewModel = ConvertTaskToTaskView(task);
            
            List<Project>? projects = projectService.GetAllProjects().Item1;
            ViewBag.Projects = projects;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Task task = taskService.GetTaskById(id, HttpContext.Session.GetInt32("userId"));

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
            string? errorMessage = taskService.DeleteTask(viewModel.Id);

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
