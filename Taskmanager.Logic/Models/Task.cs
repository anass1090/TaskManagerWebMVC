using TaskManager.Logic.Exceptions;

namespace TaskManager.Logic.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Project_Id { get; set; }
        public int? User_Id { get; set; }

        public Task(int id, string title, string description, int? project_Id, int? user_Id)
        {
            if (title != null && description != null)
            {
                Id = id;
                Title = title;
                Description = description;
                Project_Id = project_Id;
                User_Id = user_Id;
            } else
            {
                throw new TaskException("Not all required fields where filled in.");
            }
        }

        public Task(int id, string title, string description, int? projectId)
        {
            if (title != null && description != null)
            {
                Id = id;
                Title = title;
                Description = description;
                Project_Id = projectId;
            }
            else
            {
                throw new TaskException("Not all required fields where filled in.");
            }
        }
    }
}
