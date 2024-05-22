using System.Collections.Generic;
using TaskManager.Logic.Models;
#nullable enable
namespace TaskManager.Logic.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserById(int id, out string? errorMessage);
        User? CreateUser(string firstName, string lastName, string password, string email, out string? errorMessage);
        User? AuthenticateUser(string email, string password);
        User? GetUserByEmail(string email, out string? errorMessage);
        List<User>? GetAllUsers(out string? errorMessage);
    }
}
