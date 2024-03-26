using MySql.Data.MySqlClient;
using Mysqlx;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DAL.Connection;
using TaskManager.DAL.DTO_s;


namespace TaskManager.DAL.Repositories
{
    public class TaskRepository
    {
        private readonly DataAccess dataAccess;

        public TaskRepository()
        {
            dataAccess = new DataAccess(); 
        }

        public TaskDTO? CreateTask(string title, string description, out string errorMessage)
        {
            errorMessage = null;
            try
            {
                dataAccess.OpenConnection();
                string query = "INSERT INTO Tasks (Title, Description) VALUES (@Title, @Description)";

                MySqlCommand command = new MySqlCommand(query, dataAccess.Connection);

                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Description", description);

                command.ExecuteNonQuery();

                int Id = (int)command.LastInsertedId;

                TaskDTO task = new()
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

        public TaskDTO? GetTaskById(int id, out string errorMessage)
        {
            errorMessage = null;

            try {
                dataAccess.OpenConnection();

                string query = "SELECT Id, Title, Description FROM Tasks WHERE Id = @Id";

                MySqlCommand command = new(query, dataAccess.Connection);
                command.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = command.ExecuteReader();

                TaskDTO task = new();
                if (reader.Read())
                {
                    task.Id = reader.GetInt32("Id");
                    task.Title = reader.GetString("Title");
                    task.Description = reader.GetString("Description");
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

        public void UpdateTask()
        {

        }

        public void DeleteTask()
        {

        }

        public List<TaskDTO> GetAllTasks(out string errorMessage)
        {
            List<TaskDTO> tasks = [];

            errorMessage = null;

            try
            {
                dataAccess.OpenConnection();

                string query = "SELECT Id, Title, Description FROM Tasks";

                MySqlCommand command = new(query, dataAccess.Connection);
                MySqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    TaskDTO task = new()
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
