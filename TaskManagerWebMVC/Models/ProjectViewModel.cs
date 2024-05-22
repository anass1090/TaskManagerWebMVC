using TaskManager.Logic.Models;
using TaskManagerWebMVC.Models;

namespace TaskManager.MVC.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public List<Logic.Models.Task>? Tasks { get; set; }
        public List<int>? SelectedUserIds { get; set; }
    }
}
