using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DAL.DTO_s
{
    public class TaskDTO
    {
        public TaskDTO(Task task) 
        {
            Id = task.Id;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
