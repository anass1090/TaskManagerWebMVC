using TaskManagerWebMVC.Models;

namespace TaskManager.MVC.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public List<TaskViewModel>? Tasks { get; set; }
        public List<UserViewModel>? Users { get; set; }
    }
}
