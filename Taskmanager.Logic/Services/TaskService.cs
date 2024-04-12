using System.Collections.Generic;
using TaskManager.Logic.Interfaces;
using Task = TaskManager.Logic.Models.Task;
#nullable enable
namespace TaskManager.Logic.Managers
{
    public class TaskService
    {
        private readonly ITaskRepository TaskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            TaskRepository = taskRepository;
        }
        public (Task?, string) CreateTask(string title, string description, int? projectId)
        {
            Task? task = TaskRepository.CreateTask(title, description, projectId, out string errorMessage);

            return (task, errorMessage);
        }

        public (Task?, string) GetTaskById(int id)
        {
            Task? task = TaskRepository.GetTaskById(id, out string errorMessage);

            return (task, errorMessage);
        }

        public (Task?, string) UpdateTask(int id, string title, int? projectId, string description)
        {
            Task? task = TaskRepository.UpdateTask(id, title, description, projectId, out string errorMessage);

            return (task, errorMessage);
        }

        public string? DeleteTask(int id)
        {
           TaskRepository.DeleteTask(id, out string errorMessage);

            if (errorMessage != null)
            {
                return errorMessage;
            } else
            {
                return null;
            }
        }

        public (List<Task>, string) GetAllTasks()
        {
            List<Task> tasks = TaskRepository.GetAllTasks(out string errorMessage);

            return (tasks, errorMessage);
        }
    }
}
