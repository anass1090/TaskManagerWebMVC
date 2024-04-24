using System.Collections.Generic;
using TaskManager.Logic.Interfaces;
using Task = TaskManager.Logic.Models.Task;
#nullable enable
namespace TaskManager.Logic.Managers
{
    public class TaskService(ITaskRepository taskRepository)
    {
        public (Task?, string?) CreateTask(string title, string description, int? projectId, int userId)
        {
            Task? task = taskRepository.CreateTask(title, description, projectId, userId, out string? errorMessage);

            return (task, errorMessage);
        }

        public (Task?, string?) GetTaskById(int id)
        {
            Task? task = taskRepository.GetTaskById(id, out string? errorMessage);

            return (task, errorMessage);
        }

        public (Task?, string?) UpdateTask(int id, string title, int? projectId, string description)
        {
            Task? task = taskRepository.UpdateTask(id, title, description, projectId, out string? errorMessage);

            return (task, errorMessage);
        }

        public string? DeleteTask(int id)
        {
            taskRepository.DeleteTask(id, out string? errorMessage);

            if (errorMessage != null)
            {
                return errorMessage;
            } else
            {
                return null;
            }
        }

        public (List<Task>?, string?) GetAllTasks(int userId)
        {
            List<Task>? tasks = taskRepository.GetAllTasks(userId, out string? errorMessage);

            return (tasks, errorMessage);
        }
    }
}
