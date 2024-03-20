using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskmanager.Logic.Models
{
    public class Task
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Project_id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public DateTime DueDate { get; set; }
    }
}
