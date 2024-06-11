using System.Collections.Generic;
using TaskManager.Logic.Interfaces;
using Task = TaskManager.Logic.Models.Task;
using TaskManager.Logic.Exceptions;
using TaskManager.Logic.Models;
using System.Threading.Tasks;
#nullable enable
namespace TaskManager.Logic.Managers
{
    public class TaskService(ITaskRepository taskRepository)
    {
        public Task CreateTask(string title, string description, int? projectId, int userId)
        {
            if (title == null || description == null)
            {
                throw new TaskException("Title and / or description is empty.");
            }
            else if (title.Length > 50 || description.Length > 1000)
            {
                throw new TaskException("Too many characters in your title or description, try again.");
            }

            try
            {
                return taskRepository.CreateTask(title, description, projectId, userId, out string? errorMessage);
            }
            catch (TaskException)
            {
                throw new TaskException("try again later.");
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
            bool isDeleted = taskRepository.DeleteTask(id, out string? errorMessage);

            if (isDeleted == false)
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
