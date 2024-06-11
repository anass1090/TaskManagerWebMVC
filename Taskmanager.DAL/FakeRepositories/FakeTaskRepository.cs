using System;
using System.Collections.Generic;
using TaskManager.Logic.Interfaces;
using TaskManager.Logic.Models;

#nullable enable
namespace TaskManager.DAL.FakeRepositories
{
    public class FakeTaskRepository : ITaskRepository
    {
        public FakeTaskRepository()
        {
        }

        public Task? CreateTask(string title, string description, int? projectId, int userId, out string? errorMessage)
        {
            errorMessage = null;
            try
            {
                Task task = new()
                {
                    Id = 1,
                    Title = title,
                    Description = description,
                    Project_Id = projectId,
                    User_Id = userId,
                };

                return task;
                
            } catch (Exception ex) {
                errorMessage = "Error creating task: " + ex.Message;
                return null;
            }
          
        }

        public Task? GetTaskById(int id, out string? errorMessage)
        {
            errorMessage = null;

            try 
            {
                Task task = new()
                {
                    Id = 1,
                    Title = "Test title",
                    Description = "Test description",
                    Project_Id = 3,
                    User_Id = 2
                };

                return null;
            }
            catch (Exception ex) {
                errorMessage = "Error fetching task: " + ex.Message;
                return null;
            }
        }

        public Task? UpdateTask(int id, string title, string description, int? projectId, out string? errorMessage)
        {
            errorMessage = null;
            try
            {
                Task updatedTask = new()
                { 
                    Id = id,
                    Title = title,
                    Description = description,
                    Project_Id = projectId,
                };

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
                    new Task { Id = 1, Title = "Test Title 1", Description = "Test description 1" },
                    new Task { Id = 2, Title = "Test Title 2" , Description = "Test description 2" },
                    new Task { Id = 3, Title = "Test Title 3" , Description = "Test description 3" } 
                ];
                
                return tasks;
            }
            catch (Exception ex)
            {
                errorMessage = "Error fetching tasks: " + ex.Message;
                return null;
            }
        }
    }
}
