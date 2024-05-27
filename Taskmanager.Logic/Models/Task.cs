namespace TaskManager.Logic.Models
{
    public class Task
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int? Project_Id { get; set; }
        public int? User_Id { get; set; }
    }
}
