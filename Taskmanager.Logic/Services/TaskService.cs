using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DAL.DTO_s;
using TaskManager.DAL.Repositories;
using Task = TaskManager.Logic.Models.Task;

namespace TaskManager.Logic.Managers
{
    public class TaskService
    {
        private readonly TaskRepository repository;

        public TaskService()
        {
            repository = new TaskRepository();
        }

        #nullable enable
        public Task? CreateTask(string title, string description)
        {
            TaskDTO? taskDTO = repository.CreateTask(title, description);
            Task? task = new Task();
            if (taskDTO != null)
            {
                task = new()
                {
                    Title = taskDTO.Title,
                    Description = taskDTO.Description
                };
            }

            return task;
        }
        #nullable disable

        public void GetTaskById(int id)
        {
            repository.GetTaskById(id);
        }

        public void UpdateTask(int id)
        {
            repository.UpdateTask();
        }

        public void DeleteTask(int id)
        {
            repository.DeleteTask();
        }

        public List<Task> GetAllTasks()
        {
            List<TaskDTO> taskDTOs = repository.GetAllTasks();
            List<Task> tasks = new List<Task>();

            foreach (TaskDTO taskDTO in taskDTOs)
            {
                Task task = new()
                {
                    Id = taskDTO.Id,
                    Title = taskDTO.Title,
                    Description = taskDTO.Description,
                }; 
                
                tasks.Add(task);
            }
             
            return tasks;
        }
    }
}
