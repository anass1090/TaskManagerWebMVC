using System.Collections.Generic;
using TaskManager.Logic.Models;
namespace TaskManager.Logic.Interfaces
#nullable enable
{
    public interface ITaskRepository
    {
        List<Task> GetAllTasks(out string errorMessage);
        Task? GetTaskById(int id, out string errorMessage);
        Task? CreateTask(string title, string description, out string errorMessage);
        Task? UpdateTask(int id, string title, string description, out string errorMessage);
        Task DeleteTask(int id);
    }
}
