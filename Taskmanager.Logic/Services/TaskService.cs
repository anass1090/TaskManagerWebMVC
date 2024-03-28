using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManager.Logic.Interfaces;
using Task = TaskManager.Logic.Models.Task;

namespace TaskManager.Logic.Managers
{
    public class TaskService
    {
        private readonly ITaskRepository TaskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            this.TaskRepository = taskRepository;
        }

        #nullable enable
        public (Task?, string) CreateTask(string title, string description)
        {
            Task? task = TaskRepository.CreateTask(title, description, out string errorMessage);

            return (task, errorMessage);
        }

        public (Task?, string) GetTaskById(int id)
        {
            Task? task = TaskRepository.GetTaskById(id, out string errorMessage);

            return (task, errorMessage);
        }
        #nullable disable

        public void UpdateTask(int id)
        {
            TaskRepository.UpdateTask(id);
        }

        public void DeleteTask(int id)
        {
            TaskRepository.DeleteTask(id);
        }

        public (List<Task>, string) GetAllTasks()
        {
            List<Task> tasks = TaskRepository.GetAllTasks(out string errorMessage);

            return (tasks, errorMessage);
        }
    }
}
