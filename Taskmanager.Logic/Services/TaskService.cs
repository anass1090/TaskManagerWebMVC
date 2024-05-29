using System.Collections.Generic;
using TaskManager.Logic.Interfaces;
using TaskManager.Logic.Models;
using Task = TaskManager.Logic.Models.Task;
#nullable enable
namespace TaskManager.Logic.Managers
{
    public class TaskService(ITaskRepository taskRepository)
    {
        public (Task?, string?) CreateTask(string title, string description, int? projectId, int userId)
        {
            if (title == null || description == null)
            {
                return (null, "Not all required fields have been filled in, check this and try again.");
            }

            Task? task = taskRepository.CreateTask(title, description, projectId, userId, out string? errorMessage);

            if (task != null)
            {
                return (task, null);
            } else
            {
                return (null, "Something went wrong while creating the task, try again.");
            }
        }

        public (Task?, string?) GetTaskById(int id, int? userId)
        {
            Task? task = taskRepository.GetTaskById(id, out string? errorMessage);

            if (task != null && userId != null && task.User_Id == userId)
            {
                return (task, errorMessage);
            } else
            {
                return (null, "You are trying to edit a task that is not yours!");
            }
        }

        public (Task?, string?) UpdateTask(int id, string title, int? projectId, string description, int? userId)
        {
            Task? checkTask = taskRepository.GetTaskById(id, out string? errorMessage);

            if(checkTask != null && userId != null && checkTask.User_Id == userId)
            {
                Task? task = taskRepository.UpdateTask(id, title, description, projectId, out errorMessage);
                return (task, errorMessage);
            } else
            {
                return (null, "You are trying to edit a task that is not yours!");
            }
        }

        public string? DeleteTask(int id)
        {
            taskRepository.DeleteTask(id, out string? errorMessage);

            if (errorMessage != null)
            {
                return "Something went wrong while deleting the task, try again later.";
            } 
            else
            {
                return null;
            }
        }

        public (List<Task>?, string?) GetAllTasks(int userId)
        {
            List<Task>? tasks = taskRepository.GetAllTasks(userId, out string? errorMessage);

            if (tasks == null && errorMessage != null)
            {
                return (tasks, "Something went wrong while trying to fetch your tasks, try again later.");
            }
            return (tasks, errorMessage);
        }
    }
}
