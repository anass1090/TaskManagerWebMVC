using System.Collections.Generic;
using TaskManager.Logic.Models;
namespace TaskManager.Logic.Interfaces
#nullable enable
{
    public interface ITaskRepository
    {
        List<Task>? GetAllTasks(int userId, out string? errorMessage);
        Task GetTaskById(int id);
        Task CreateTask(string title, string description, int? projectId, int userId);
        Task? UpdateTask(int id, string title, string description, int? projectId, out string? errorMessage);
        bool DeleteTask(int id, out string? errorMessage);
    }
}
