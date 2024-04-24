using System.Web;
using System.Xml;
using TaskManager.Logic.Interfaces;
using TaskManager.Logic.Models;
#nullable enable
namespace TaskManager.Logic.Services
{
    public class UserService
    {
        private readonly IUserRepository UserRepository;

        public UserService(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public (User?, string?) CreateUser(string firstName, string lastName, string email, string password)
        {
            User? user = UserRepository.CreateUser(firstName, lastName, password, email, out string? errorMessage);           

            return (user, errorMessage);
        }

        public (User?, string?) AuthenticateUser(string email, string password)
        {
            User? user = UserRepository.AuthenticateUser(email, password);
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
            User? user = UserRepository.GetUserByEmail(email, out string? errorMessage);

            return (user, errorMessage);
        }

        public (User?, string?) GetUserById(int id)
        {
            User? user = UserRepository.GetUserById(id, out string? errorMessage);

            return (user, errorMessage);
        }
    }
}
