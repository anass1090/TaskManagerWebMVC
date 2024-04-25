using TaskManager.Logic.Interfaces;
using TaskManager.Logic.Models;
#nullable enable
namespace TaskManager.Logic.Services
{
    public class UserService(IUserRepository userRepository)
    {
        public (User?, string?) CreateUser(string firstName, string lastName, string email, string password)
        {
            User? user = userRepository.CreateUser(firstName, lastName, password, email, out string? errorMessage);           

            return (user, errorMessage);
        }

        public (User?, string?) AuthenticateUser(string email, string password)
        {
            User? user = userRepository.AuthenticateUser(email, password);
            string? errorMessage = null;

            if (user == null)
            {
                errorMessage = "Invalid email and or password.";
                return (null, errorMessage);
            }

            return (user, errorMessage);
        }

        public (User?, string?) GetUserByEmail(string email)
        {
            User? user = userRepository.GetUserByEmail(email, out string? errorMessage);

            return (user, errorMessage);
        }

        public (User?, string?) GetUserById(int id)
        {
            User? user = userRepository.GetUserById(id, out string? errorMessage);

            return (user, errorMessage);
        }
    }
}
