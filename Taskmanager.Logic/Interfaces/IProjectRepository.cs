using System.Collections.Generic;
using TaskManager.Logic.Models;

#nullable enable
namespace TaskManager.Logic.Interfaces
{
    public interface IProjectRepository
    {
        List<Project>? GetAllProjects(out string? errorMessage);
        Project? GetProjectById(int id, out string? errorMessage);
        Project? CreateProject(string title, string description, List<int>? userIdsToAdd);
        Project? UpdateProject(int id, string title, string description, List<int>? userIdsToAdd, List<int>? userIdsToRemove, out string? errorMessage);
        void DeleteProject(int id, out string? errorMessage);
        (List<Task>?, Project?) GetTasksByProject(int projectId, out string? errorMessage);
        void DeleteUserFromProject(int projectId, int userId, out string? errorMessage);
        void AddUserToProject(int projectId, int userId, out string? errorMessage);
    }
}
