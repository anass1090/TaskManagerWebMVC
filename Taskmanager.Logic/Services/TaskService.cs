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
        public (Task?, string) CreateTask(string title, string description)
        {
            TaskDTO? taskDTO = repository.CreateTask(title, description, out string errorMessage);
            Task? task = new();
            if (taskDTO != null)
            {
                task = new()
                {
                    Title = taskDTO.Title,
                    Description = taskDTO.Description
                };
            }

            return (task, errorMessage);
        }

        public (Task, string) GetTaskById(int id)
        {
            TaskDTO? taskDTO = repository.GetTaskById(id, out string errorMessage);
            Task? task = new();

            if (taskDTO != null)
            {
                task = new()
                {
                    Id = taskDTO.Id,
                    Title = taskDTO.Title,
                    Description = taskDTO.Description
                };
            }

            return (task, errorMessage);
        }
        #nullable disable

        public void UpdateTask(int id)
        {
            repository.UpdateTask();
        }

        public void DeleteTask(int id)
        {
            repository.DeleteTask();
        }

        public (List<Task>, string) GetAllTasks()
        {
            List<TaskDTO> taskDTOs = repository.GetAllTasks(out string errorMessage);
            List<Task> tasks = [];

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
             
            return (tasks, errorMessage);
        }
    }
}
