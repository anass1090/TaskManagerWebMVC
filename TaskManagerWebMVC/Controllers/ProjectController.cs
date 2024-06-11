using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.Services;
using TaskManager.Logic.Models;
using TaskManager.MVC.Models;

namespace TaskManager.MVC.Controllers
{
    public class ProjectController(ProjectService projectService, UserService userService) : Controller
    {
        [HttpGet]
        public object Index()
        {
            (List<Project>? projects, string? errorMessage) = projectService.GetAllProjects();

            if (errorMessage != null)
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction(nameof(Index), "Home", null);
            }

            List<ProjectViewModel> projectViewModels = [];

            if (projects != null)
            {
                foreach (Project? project in projects)
                {
                    if (project != null)
                    {
                        ProjectViewModel? projectView = ConvertProjectToProjectView(project);

                        if (projectView != null)
                        {
                            projectViewModels.Add(projectView);
                        }
                    }
                }
            }

            return View(projectViewModels);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
           (List<User>? users, string? errorMessage) = userService.GetAllUsers();

            ViewBag.Users = users;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectViewModel model)
        {
            Project? project; string message;

            if (model.SelectedUserIds != null)
            {
                (project, message) = projectService.CreateProject(model.Title, model.Description, model.SelectedUserIds);
            } else
            {
                (project, message) = projectService.CreateProject(model.Title, model.Description, null);
            }

            if (project == null)
            {
                ViewBag.ErrorMessage = message;
            }
            else
            {
                TempData["successMessage"] = message;

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private static ProjectViewModel? ConvertProjectToProjectView(Project project)
        {
            if (project.Title != null && project.Description != null)
            {

                ProjectViewModel projectView = new()
                {
                    Id = project.Id,
                    Title = project.Title,
                    Description = project.Description,
                };

                return projectView;
            }

            return null;
        }
    }
}
