﻿using System.Collections.Generic;
#nullable enable
namespace TaskManager.Logic.Models
{
    public class User
    {
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public List<Project>? Projects { get; set; }
    }
}
