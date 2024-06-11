using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TaskManager.DAL.Connection;
using TaskManager.Logic.Models;
using TaskManager.Logic.Interfaces;
#nullable enable
namespace TaskManager.DAL.FakeRepositories
{
    public class FakeProjectRepository : IProjectRepository
    {
        private readonly DataAccess dataAccess;

        public FakeProjectRepository()
        {
            dataAccess = new();
        }

        public Project? CreateProject(string title, string description, List<int>? userIdsToAdd)
        {
            try
            {
                dataAccess.OpenConnection();
                
                string query = "INSERT INTO Projects (Title, Description) VALUES (@Title, @Description)";

                using MySqlCommand command = new (query, dataAccess.Connection);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Description", description);
                command.ExecuteNonQuery();

                int Id = (int)command.LastInsertedId;

                Project project = new()
                {
                    Id = Id,
                    Title = title,
                    Description = description
                };

                if(userIdsToAdd != null)
                {
                    foreach (int userId in userIdsToAdd)
                    {
                        AddUserToProject(project.Id ,userId, out _);
                    }
                }

                project.Users = GetUsersByProjectId(project.Id, out _);

                return project;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error creating project: {ex.Message}");
                return null;
            }
            finally
            {
                dataAccess.CloseConnection();
            }
        }

        public Project? GetProjectById(int id, out string? errorMessage)
        {
            errorMessage = null;

            try
            {
                dataAccess.OpenConnection();

                string query = "SELECT Id, Title, Description FROM Projects WHERE Id = @Id";

                MySqlCommand command = new(query, dataAccess.Connection);
                command.Parameters.AddWithValue("@Id", id);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Project project = new()
                    {
                        Id = reader.GetInt32("Id"),
                        Title = reader.GetString("Title"),
                        Description = reader.GetString("Description")
                    };

                    return project;
                }
                return null;
            }
            catch (MySqlException ex)
            {
                errorMessage = "Error fetching project: " + ex.Message;
                return null;

            }
            finally
            {
                dataAccess.CloseConnection();
            }
        }

        public Project? UpdateProject(int id, string title, string description, List<int>? userIdsToAdd, List<int>? userIdsToRemove, out string? errorMessage)
        {
            errorMessage = null;
            try
            {
                dataAccess.OpenConnection();

                string query = "UPDATE Projects SET Title = @Title, Description = @Description WHERE Id = @Id";
                MySqlCommand command = new(query, dataAccess.Connection);

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Description", description);

                command.ExecuteNonQuery();

                Project updatedProject = new()
                {
                    Id = id,
                    Title = title,
                    Description = description
                };

                return updatedProject;

            }
            catch (MySqlException ex)
            {
                errorMessage = "Error updating project: " + ex.Message;
                return null;
            }
            finally
            {
                dataAccess.CloseConnection();
            }
        }

        public void DeleteProject(int id, out string? errorMessage)
        {
            errorMessage = null;

            try
            {
                dataAccess.OpenConnection();

                string query = "DELETE FROM Projects WHERE Id = @Id";

                MySqlCommand command = new(query, dataAccess.Connection);

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                errorMessage = "Error deleting project: " + ex.Message;
            }
            finally
            {
                dataAccess.CloseConnection();
            }
        }

        public List<Project>? GetAllProjects(out string? errorMessage)
        {
            List<Project> projects = [];

            errorMessage = null;

            try
            {
                dataAccess.OpenConnection();

                string query = "SELECT Id, Title, Description FROM Projects";

                MySqlCommand command = new(query, dataAccess.Connection);

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Project project = new()
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader["Title"].ToString(),
                        Description = reader["Description"].ToString(),
                    };

                    projects.Add(project);
                }
                return projects;
            }
            catch (MySqlException ex)
            {
                errorMessage = "Error fetching projects: " + ex.Message;
                return null;
            }
            finally
            {
                dataAccess.CloseConnection();
            }

        }

        public (List<Task>?, Project?) GetTasksByProject(int projectId, out string? errorMessage)
        {
            errorMessage = null;

            try
            {
                dataAccess.OpenConnection();

                string tasksQuery = "SELECT Id, Title, Description FROM Tasks WHERE Project_Id = @projectId";

                MySqlCommand tasksCommand = new(tasksQuery, dataAccess.Connection);
                tasksCommand.Parameters.AddWithValue("@Project_Id", projectId); 
                using MySqlDataReader tasksReader = tasksCommand.ExecuteReader();

                List<Task> tasks = [];

                while (tasksReader.Read())
                {
                    Task task = new(tasksReader.GetInt32("id"), tasksReader["Title"].ToString(), tasksReader["Description"].ToString(), projectId);

                    tasks.Add(task);
                }

                return (null, null);
            }
            catch (MySqlException ex)
            {
                errorMessage = "Error fetching tasks for project: " + ex.Message;
                return (null, null);
            }
            finally 
            { 
                dataAccess.CloseConnection(); 
            }
        }

        public void DeleteUserFromProject(int projectId, int userId, out string? errorMessage)
        {
            errorMessage = null;
            try
            {
                string query = "DELETE FROM UsersProjects WHERE ProjectId = @ProjectId AND UserId = @UserId";
                using MySqlCommand command = new(query, dataAccess.Connection);
                command.Parameters.AddWithValue("@ProjectId", projectId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                errorMessage = "Error removing user from project: " + ex.Message;
            }
        }

        public void AddUserToProject(int projectId, int userId, out string? errorMessage)
        {
            errorMessage = null;
            try
            {
                string query = "INSERT INTO usersprojects (project_id, user_id) VALUES (@ProjectId, @UserId)";
                using MySqlCommand command = new(query, dataAccess.Connection);

                command.Parameters.AddWithValue("@ProjectId", projectId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                errorMessage = "Error adding users to project: " + ex.Message;
            }
        }

        public List<User>? GetUsersByProjectId(int projectId, out string? errorMessage)
        {
            errorMessage = null;
            List<User> users = [];
            try
            {
                string query = "SELECT u.Id, u.FirstName, u.LastName, u.Email FROM Users u INNER JOIN UsersProjects up ON u.Id = up.UserId WHERE up.ProjectId = @ProjectId";

                using MySqlCommand command = new(query, dataAccess.Connection);
                command.Parameters.AddWithValue("@ProjectId", projectId);

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    User user = new()
                    {
                        Id = reader.GetInt32("Id"),
                        FirstName = reader.GetString("Name"),
                        Email = reader.GetString("Email"),
                        LastName = reader.GetString("LastName"),
                        Password = reader.GetString("Password")
                    };
                    users.Add(user);
                }

                return users;
            }
            catch (MySqlException ex)
            {
                errorMessage = "Error fetching users for project: " + ex.Message;
                return null;
            }
        }
    }
}
