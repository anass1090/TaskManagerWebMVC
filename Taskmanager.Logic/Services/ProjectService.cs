using System.Collections.Generic;
using TaskManager.Logic.Interfaces;
using TaskManager.Logic.Models;
#nullable enable
namespace TaskManager.Logic.Services
{
    public class ProjectService(IProjectRepository projectRepository)
    {
        public (Project?, string) CreateProject(string title, string description, List<int> selectedUserIds)
        {
            Project? project = projectRepository.CreateProject(title, description, selectedUserIds);
            string errorMessage;

            if (project == null)
            {
                errorMessage = "Something went wrong creating the project.";
                return (null, errorMessage);
            }

            return (project, "Project created successfully.");
        }

        public (List<Project>?, string?) GetAllProjects()
        {
            List<Project>? projects = projectRepository.GetAllProjects(out string? errorMessage);

            if (errorMessage != null)
            {
                return (null, "Something went wrong while fetching your projects. Try again later.");
            }

            return (projects, null);
        }

        public (Project?, string?) GetProjectById(int id)
        {
            Project? project = projectRepository.GetProjectById(id, out string? errorMessage);

            return (project, errorMessage);
        }

        public (Project?, string?) UpdateProject(int id, string title, string description, List<int>? userIdsToAdd, List<int>? userIdsToRemove, out string? errorMessage)
        {
            Project? project = projectRepository.UpdateProject(id, title, description, userIdsToAdd, userIdsToRemove, out errorMessage);

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
