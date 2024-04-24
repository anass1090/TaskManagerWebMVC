using MySql.Data.MySqlClient;
using System;
using TaskManager.DAL.Connection;
using TaskManager.Logic.Interfaces;
using TaskManager.Logic.Models;
#nullable enable
namespace TaskManager.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataAccess dataAccess;

        public UserRepository()
        {
            dataAccess = new();
        }

        public User? CreateUser(string firstName, string lastName, string password, string email, out string? errorMessage)
        {
            errorMessage = null;

            try
            {
                dataAccess.OpenConnection();

                string query = "INSERT INTO Users (FirstName, LastName, Password, Email) VALUES (@FirstName, @LastName, @Password, @Email)";

                MySqlCommand command = new(query, dataAccess.Connection);

                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Email", email);

                command.ExecuteNonQuery();

                int Id = (int)command.LastInsertedId;

                User? user = new()
                {
                    Id = Id,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = password,
                };

                return user;
            }
            catch (Exception ex)
            {
                errorMessage = "Error creating user: " + ex.Message;
                return null;
            }
            finally
            {
                dataAccess.CloseConnection();
            }
        }

        public User? AuthenticateUser(string email, string password)
        {
            User? user = GetUserByEmail(email, out _);

            if (user != null && user.Password == password)
            {
                return user;
            } else
            {
                return null;
            }
        }

        public User? GetUserByEmail(string email, out string? errorMessage)
        {
            errorMessage = null;

            try
            {
                dataAccess.OpenConnection();

                string query = "SELECT Id, FirstName, LastName, Email, Password FROM Users WHERE Email = @Email";

                MySqlCommand command = new(query, dataAccess.Connection);
                command.Parameters.AddWithValue("@Email", email);

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    User user = new()
                    {
                        Id = reader.GetInt32("Id"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        Email = reader.GetString("Email"),
                        Password = reader.GetString("Password")
                    };

                    return user;
                }

                return null;
            }
            catch (Exception ex)
            {
                errorMessage = "Error retrieving user: " + ex.Message;
                return null;
            }
            finally
            {
                dataAccess.CloseConnection();
            }
        }

        public User? GetUserById(int id, out string? errorMessage)
        {
            errorMessage = null;

            try
            {
                dataAccess.OpenConnection();

                string query = "SELECT Id, FirstName, LastName, Email, Password FROM Users WHERE Id = @Id";

                MySqlCommand command = new(query, dataAccess.Connection);
                command.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    User? user = new()
                    {
                        Id = reader.GetInt32("Id"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        Email = reader.GetString("Email"),
                        Password = reader.GetString("Password")
                    };   
                    
                    return user;
                }

                return null;
            }
            catch (Exception ex)
            {
                errorMessage = "Error retrieving user: " + ex.Message;
                return null;
            }
            finally
            {
                dataAccess.CloseConnection();
            }
        }
    }
}
