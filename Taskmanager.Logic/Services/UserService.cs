using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            User? user = UserRepository.AuthenticateUser(email, password, out string? errorMessage);

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
