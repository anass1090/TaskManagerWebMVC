using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using TaskManager.DAL.Connection;
using TaskManager.Logic.Interfaces;
using TaskManager.Logic.Models;


namespace TaskManager.DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DataAccess dataAccess;

        public TaskRepository()
        {
            dataAccess = new();
        }

        public Task CreateTask(string title, string description, int? projectId, out string errorMessage)
        {
            errorMessage = null;
            try
            {
                dataAccess.OpenConnection();
                string query = "INSERT INTO Tasks (Title, Description, Project_Id) VALUES (@Title, @Description, @Project_Id)";

                MySqlCommand command = new (query, dataAccess.Connection);

                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Project_Id", projectId);

                command.ExecuteNonQuery();

                int Id = (int)command.LastInsertedId;

                Task task = new()
                {
                    Id = Id,
                    Title = title,
                    Description = description
                };

                return task;
                

            } catch (Exception ex) {
                errorMessage = "Error creating task: " + ex.Message;
                return null;
            }
            finally {
                dataAccess.CloseConnection();
            }
        }

        public Task GetTaskById(int id, out string errorMessage)
        {
            errorMessage = null;

            try {
                dataAccess.OpenConnection();

                string query = "SELECT Id, Title, Description, Project_Id FROM Tasks WHERE Id = @Id";

                MySqlCommand command = new(query, dataAccess.Connection);
                command.Parameters.AddWithValue("@Id", id);

                using MySqlDataReader reader = command.ExecuteReader();

                Task task = new();
                if (reader.Read())
                {
                    task.Id = reader.GetInt32("Id");
                    task.Title = reader.GetString("Title");
                    task.Description = reader.GetString("Description");
                    task.Project_Id = reader["Project_Id"] as Int32?;
                }

                return task;
            }
            catch (Exception ex) {
                errorMessage = "Error fetching task: " + ex.Message;
                return null;

            }
            finally { 
                dataAccess.CloseConnection(); 
            }
        }

        public Task UpdateTask(int id, string title, string description, int? projectId, out string errorMessage)
        {
            errorMessage = null;
            try
            {
                dataAccess.OpenConnection();

                string query = "UPDATE Tasks SET Title = @Title, Description = @Description, Project_Id = @Project_Id WHERE Id = @Id";
                MySqlCommand command = new(query, dataAccess.Connection);

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Project_Id", projectId);

                command.ExecuteNonQuery();

                Task updatedTask = new() { 
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
            finally
            {
                dataAccess.CloseConnection();
            }
        }

        public void DeleteTask(int id, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                dataAccess.OpenConnection();
                
                string query = "DELETE FROM Tasks WHERE Id = @Id";

                MySqlCommand command = new(query, dataAccess.Connection);

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            } catch(Exception ex)
            {
                errorMessage = "Error deleting task: " + ex.Message;
            } finally { 
                dataAccess.CloseConnection(); 
            }
        }

        public List<Task> GetAllTasks(out string errorMessage)
        {
            List<Task> tasks = [];

            errorMessage = null;

            try
            {
                dataAccess.OpenConnection();

                string query = "SELECT Id, Title, Description FROM Tasks";

                MySqlCommand command = new(query, dataAccess.Connection);

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Task task = new()
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader["Title"].ToString(),
                        Description = reader["Description"].ToString(),
                    };

                    tasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Error fetching tasks: " + ex.Message;
            }
            finally
            {
                dataAccess.CloseConnection();
            }

            return tasks;
        }
    }
}
