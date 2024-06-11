using System.ComponentModel.DataAnnotations;

namespace TaskManagerWebMVC.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public int? Project_Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}
