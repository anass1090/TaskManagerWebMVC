using System;
using System.Collections.Generic;
using TaskManager.Logic.Exceptions;
using TaskManager.Logic.Interfaces;
using TaskManager.Logic.Models;

#nullable enable
namespace TaskManager.DAL.FakeRepositories
{
    public class FakeTaskRepository : ITaskRepository
    {
        private readonly Dictionary<int, Task> Tasks = new()
        {
            { 1, new Task(1, "Test title", "Test description", 3, 1) },
            { 2, new Task(2, "Another title", "Another description", 3, 2) }
        };

        public Task CreateTask(string title, string description, int? projectId, int userId)
        {
            try
            {
                Task task = new(1, title, description, projectId, userId);

                if (userId == 999)
                {
                    throw new DatabaseException();
                }
                return task;
                
            } catch (DatabaseException) {
                throw new DatabaseException();
            }
          
        }

        public Task GetTaskById(int id)
        {
            try
            {
                if (id == 999)
                {
                    throw new DatabaseException();
                }

                Tasks.TryGetValue(id, out Task? task);

                if (task == null)
                {
                    throw new TaskException("Task not found.");
                }
                
                return task;
            }
            catch (DatabaseException)
            {
                throw new DatabaseException();
            }
        }

        public Task? UpdateTask(int id, string title, string description, int? projectId, out string? errorMessage)
        {
            errorMessage = null;
            try
            {
                Task updatedTask = new(id, title, description, projectId, null);

                return updatedTask;

            } catch(Exception ex)
            {
                errorMessage = "Error updating task: " + ex.Message;
                return null;
            }
        }

        public bool DeleteTask(int id, out string? errorMessage)
        {
            errorMessage = null;
               
            if (id == 1)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public List<Task>? GetAllTasks(int userId, out string? errorMessage)
        {
            errorMessage = null;

            try
            {
                List<Task> tasks =
                [
                    new Task(1, "Test Title 1", "Test description 1", null, null),
                    new Task(2, "Test Title 2" , "Test description 2", null, null),
                    new Task(3, "Test Title 3" , "Test description 3", null, null) 
                ];
                
                return tasks;
            }
            catch (Exception ex)
            {
                errorMessage = "Error fetching tasks: " + ex.Message;
                return null;
            }
        }

        private static Task CreateTaskObject(int id, string title, string description, int? projectId, int? userId)
        {
            return new Task(id, title, description, projectId, userId);
        }
    }
}
