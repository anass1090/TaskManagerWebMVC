using System.Collections.Generic;
#nullable enable
namespace TaskManager.Logic.Models
{
    public class Project
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public List<Task>? Tasks { get; set; }
        public List<User>? Users { get; set; }
    }
}
