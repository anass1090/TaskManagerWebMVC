﻿using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.Managers;
using TaskManagerWebMVC.Models;
using Task = TaskManager.Logic.Models.Task;
using TaskManager.Logic.Models;
using TaskManager.Logic.Services;

namespace TaskManager.MVC.Controllers
{
    public class TaskController(TaskService taskService, ProjectService projectService) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                TempData["errorMessage"] = "something went wrong try logging in again.";
                return RedirectToAction("Login", "Login");
            }
            else
            {
                (List<Task>? tasks, string? errorMessage) = taskService.GetAllTasks(userId.Value);

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
            Task? task = taskService.GetTaskById(id).Item1;
            string? errorMessage = taskService.GetTaskById(id).Item2;

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
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                TempData["errorMessage"] = "something went wrong try logging in again.";
                return RedirectToAction("Login", "Login");
            }

            string? errorMessage = taskService.CreateTask(title, description, projectId, userId.Value).Item2;

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
            List<Project>? projects = projectService.GetAllProjects().Item1;

            ViewBag.Projects = projects;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaskViewModel viewModel)
        {
            string? errorMessage = taskService.UpdateTask(viewModel.Id, viewModel.Title, viewModel.Project_Id, viewModel.Description).Item2;

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
            (Task? task, string? errorMessage) = taskService.GetTaskById(id);

            if (errorMessage != null)
            {
                ViewBag.ErrorMessage = errorMessage;
            }

            if (task == null)
            {
                return NotFound();
            }

            TaskViewModel viewModel = ConvertTaskToTaskView(task);
            
            List<Project>? projects = projectService.GetAllProjects().Item1;
            ViewBag.Projects = projects;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Task? task = taskService.GetTaskById(id).Item1;

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
