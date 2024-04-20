using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TaskManager.DAL.Connection;
using TaskManager.Logic.Models;
using TaskManager.Logic.Interfaces;

namespace TaskManager.DAL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataAccess dataAccess;

        public ProjectRepository()
        {
            dataAccess = new();
        }

        public Project CreateProject(string title, string description, out string errorMessage)
        {
            errorMessage = null;
            try
            {
                dataAccess.OpenConnection();
                string query = "INSERT INTO Projects (Title, Description) VALUES (@Title, @Description)";

                MySqlCommand command = new (query, dataAccess.Connection);

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

                return project;


            }
            catch (Exception ex)
            {
                errorMessage = "Error creating project: " + ex.Message;
                return null;
            }
            finally
            {
                dataAccess.CloseConnection();
            }
        }

        public Project GetProjectById(int id, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                dataAccess.OpenConnection();

                string query = "SELECT Id, Title, Description FROM Projects WHERE Id = @Id";

                MySqlCommand command = new(query, dataAccess.Connection);
                command.Parameters.AddWithValue("@Id", id);

                using MySqlDataReader reader = command.ExecuteReader();

                Project project = new();
                if (reader.Read())
                {
                    project.Id = reader.GetInt32("Id");
                    project.Title = reader.GetString("Title");
                    project.Description = reader.GetString("Description");
                }

                return project;
            }
            catch (Exception ex)
            {
                errorMessage = "Error fetching project: " + ex.Message;
                return null;

            }
            finally
            {
                dataAccess.CloseConnection();
            }
        }

        public Project UpdateProject(int id, string title, string description, out string errorMessage)
        {
            errorMessage = null;
            try
            {
                dataAccess.OpenConnection();

                string query = "UPDATE Tasks SET Title = @Title, Description = @Description WHERE Id = @Id";
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
            catch (Exception ex)
            {
                errorMessage = "Error updating project: " + ex.Message;
                return null;
            }
            finally
            {
                dataAccess.CloseConnection();
            }
        }

        public void DeleteProject(int id, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                dataAccess.OpenConnection();

                string query = "DELETE FROM Tasks WHERE Id = @Id";

                MySqlCommand command = new(query, dataAccess.Connection);

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errorMessage = "Error deleting project: " + ex.Message;
            }
            finally
            {
                dataAccess.CloseConnection();
            }
        }

        public List<Project> GetAllProjects(out string errorMessage)
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
            }
            catch (Exception ex)
            {
                errorMessage = "Error fetching tasks: " + ex.Message;
            }
            finally
            {
                dataAccess.CloseConnection();
            }

            return projects;
        }
    }
}
