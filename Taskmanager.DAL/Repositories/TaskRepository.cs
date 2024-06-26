﻿using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using TaskManager.Logic.Interfaces;
using TaskManager.Logic.Models;
using TaskManager.Logic.Exceptions;

#nullable enable
namespace TaskManager.DAL.Repositories
{
    public class TaskRepository(string connectionString) : ITaskRepository
    {
        private readonly MySqlConnection dataAccess = new(connectionString);

        public Task CreateTask(string title, string description, int? projectId, int userId)
        {
            try
            {
                dataAccess.Open();
                string query = "INSERT INTO Tasks (Title, Description, Project_Id, User_Id) VALUES (@Title, @Description, @Project_Id, @User_Id)";

                using MySqlCommand command = new (query, dataAccess);

                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Project_Id", projectId);
                command.Parameters.AddWithValue("@User_Id", userId);

                int Id = (int)command.LastInsertedId;

                Task task = new(
                    Id, 
                    title,
                    description, 
                    projectId,
                    userId
                );

                command.ExecuteNonQuery();

                return task;
                
            } catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw new DatabaseException("Something went wrong, contact customer support.");
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new TaskException();
            }
            finally {
                dataAccess.Close();
            }
        }

        public Task GetTaskById(int id)
        {
            try 
            {
                dataAccess.Open();

                string query = "SELECT Id, Title, Description, Project_Id, User_Id FROM Tasks WHERE Id = @Id";

                MySqlCommand command = new(query, dataAccess);
                command.Parameters.AddWithValue("@Id", id);

                using MySqlDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    Task task = new(
                        reader.GetInt32("Id"),
                        reader.GetString("Title"),
                        reader.GetString("Description"),
                        reader["Project_Id"] as Int32?,
                        reader["User_Id"] as Int32?
                    );

                    return task;
                }

                throw new Exception("try again later.");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw new DatabaseException("contact customer support.");
            }
            catch (TaskException ex)
            {
                throw new TaskException(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new TaskException();
            }
            finally 
            { 
                dataAccess.Close(); 
            }
        }

        public Task? UpdateTask(int id, string title, string description, int? projectId, out string? errorMessage)
        {
            errorMessage = null;
            try
            {
                dataAccess.Open();

                string query = "UPDATE Tasks SET Title = @Title, Description = @Description, Project_Id = @Project_Id WHERE Id = @Id";
                using MySqlCommand command = new(query, dataAccess);

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Project_Id", projectId);

                command.ExecuteNonQuery();

                Task updatedTask = new(id, title, description, projectId, null);

                return updatedTask;

            } catch(Exception ex)
            {
                errorMessage = "Error updating task: " + ex.Message;
                return null;
            }
            finally
            {
                dataAccess.Close();
            }
        }

        public bool DeleteTask(int id, out string? errorMessage)
        {
            errorMessage = null;

            try
            {
                dataAccess.Open();
                
                string query = "DELETE FROM Tasks WHERE Id = @Id";

                using MySqlCommand command = new(query, dataAccess);

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();

                return true;
            } catch(MySqlException ex)
            {
                errorMessage = "Error deleting task: " + ex.Message;
                return false;
            } finally { 
                dataAccess.Close(); 
            }
        }

        public List<Task>? GetAllTasks(int userId, out string? errorMessage)
        {
            List<Task> tasks = [];

            errorMessage = null;

            try
            {
                dataAccess.Open();

                string query = "SELECT Id, Title, Description FROM Tasks WHERE User_Id = @userId";

                using MySqlCommand command = new(query, dataAccess);
                command.Parameters.AddWithValue("@userId", userId);

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Task task = new(reader.GetInt32("id"), reader["Title"].ToString(), reader["Description"].ToString(), null, null);

                    tasks.Add(task);
                }

                return tasks;
            }
            catch (MySqlException ex)
            {
                errorMessage = "Error fetching tasks: " + ex.Message;
                return null;
            }
            finally
            {
                dataAccess.Close();
            }
        }
    }
}
