using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskmanager.DAL.Repositories;

namespace Taskmanager.Logic.Managers
{
    public class TaskManager
    {
        private readonly TaskRepository repository;

        public void Create()
        {
            repository.Create();
        }

        public void Read(int id)
        {
            repository.Read(id);
        }

        public void Update(int id)
        {
            repository.Update(id);
        }

        public void Delete(int id)
        {
            repository.Delete(id);
        }

        public void GetAll()
        {
            repository.GetAll();
        }
    }
}
