namespace TaskManager.MVC.Models
{
    public class UserViewModel
    {
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public List<ProjectViewModel>? Projects { get; set; }
    }
}
