using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManager.Logic.Models;
namespace TaskManager.Logic.Interfaces
{
    public interface ITaskRepository
    {
        List<Task> GetAllTasks(out string errorMessage);
        Task? GetTaskById(int id, out string errorMessage);
        Task? CreateTask(string title, string description, out string errorMessage);
        Task UpdateTask(int id);
        Task DeleteTask(int id);
    }
}
