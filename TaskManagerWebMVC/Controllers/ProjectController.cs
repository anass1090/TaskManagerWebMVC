using Microsoft.AspNetCore.Mvc;
using TaskManager.DAL.Repositories;
using TaskManager.Logic.Services;
using TaskManager.Logic.Models;
using TaskManager.MVC.Models;

namespace TaskManager.MVC.Controllers
{
    public class ProjectController(ProjectService projectService) : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            (List<Project>? projects, string? errorMessage) = projectService.GetAllProjects();

            ViewBag.ErrorMessage = errorMessage;

            List<ProjectViewModel> projectViewModels = [];

            if (projects != null)
            {
                foreach (Project? project in projects)
                {
                    if (project != null)
                    {
                        ProjectViewModel projectView = ConvertProjectToProjectView(project);
                        projectViewModels.Add(projectView);
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        private static ProjectViewModel ConvertProjectToProjectView(Project project)
        {
            ProjectViewModel projectView = new()
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
            };

            return projectView;
        }
    }
}
