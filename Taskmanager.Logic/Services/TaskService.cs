using System.Collections.Generic;
using TaskManager.Logic.Interfaces;
using Task = TaskManager.Logic.Models.Task;
using TaskManager.Logic.Exceptions;
using System;

#nullable enable
namespace TaskManager.Logic.Managers
{
    public class TaskService(ITaskRepository taskRepository)
    {
        public Task CreateTask(string title, string description, int? projectId, int userId)
        {
            try
            {
                if (title == null || description == null)
                {
                    throw new TaskException("Title and / or description is empty.");
                }
                else if (title.Length > 50 || description.Length > 1000)
                {
                    throw new TaskException("Too many characters in your title or description, try again.");
                }

                return taskRepository.CreateTask(title, description, projectId, userId);
            }
            catch (DatabaseException)
            {
                throw new DatabaseException("Something went wrong, contact customer support.");
            }
        }

        public Task GetTaskById(int id, int? userId)
        {
            try
            {
                if (id <= 0)
                {
                    throw new TaskException("Task does not exist.");
                }
                if (userId == null)
                {
                    throw new UserException("User is not logged in.");
                }

                Task task = taskRepository.GetTaskById(id);

                if (task.User_Id != userId)
                {
                    throw new UserException("User does not have access to this task.");
                }

                return task;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex.Message);
            }
            catch (TaskException ex)
            {
                throw new TaskException(ex.Message);
            }
            catch (UserException ex)
            {
                throw new UserException(ex.Message);
            }
            catch (Exception)
            {
                throw new TaskException("An error occurred while retrieving the task.");
            }
        }

        public (Task?, string?) UpdateTask(int id, string title, int? projectId, string description, int? userId)
        {
            Task? checkTask = taskRepository.GetTaskById(id);

            if(checkTask != null && userId != null && checkTask.User_Id == userId)
            {
                Task? task = taskRepository.UpdateTask(id, title, description, projectId, out string? errorMessage);
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
