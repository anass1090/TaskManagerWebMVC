namespace TaskManager.Logic.Models
{
    public class Task
    {
        // adding own errorhandling in constructor , think about what is better
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int? Project_Id { get; set; }
        public int? User_Id { get; set; }

        //public Task(int id, string title, string description, int? project_Id, int? user_Id)
        //{
        //    Id = id;
        //    Title = title;
        //    Description = description;
        //    Project_Id = project_Id;
        //    User_Id = user_Id;
        //}
    }
}
