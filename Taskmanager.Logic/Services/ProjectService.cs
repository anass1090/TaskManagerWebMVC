using System.Collections.Generic;
using TaskManager.Logic.Interfaces;
using TaskManager.Logic.Models;
#nullable enable
namespace TaskManager.Logic.Services
{
    public class ProjectService(IProjectRepository projectRepository)
    {
        public (Project?, string?) CreateProject(string title, string description, out string? errorMessage)
        {
            Project? project = projectRepository.CreateProject(title, description, out errorMessage);

            return (project, errorMessage);
        }

        public (List<Project>?, string?) GetAllProjects()
        {
            List<Project>? projects = projectRepository.GetAllProjects(out string? errorMessage);

            return (projects, errorMessage);
        }

        public (Project?, string?) GetProjectById(int id)
        {
            Project? project = projectRepository.GetProjectById(id, out string? errorMessage);

            return (project, errorMessage);
        }

        public (Project?, string?) UpdateProject(int id,  string title, string description, out string? errorMessage)
        {
            Project? project = projectRepository.UpdateProject(id, title, description, out errorMessage);

            return (project, errorMessage);
        }

        public string? DeleteProject(int id)
        {
            projectRepository.DeleteProject(id, out string? errorMessage);

            if (errorMessage != null)
            {
                return errorMessage;
            }
            else
            {
                return null;
            }

        }
    }
}
