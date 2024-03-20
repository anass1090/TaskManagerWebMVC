using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskmanager.Logic.Managers
{
    public class ProjectManager
    {
        private readonly ProjectRepository repository;
        public Models.Task Create()
        {
            repository.Create();
        }
        public Models.Task Read()
        {

        }

        public Models.Task Update()
        {

        }

        public Models.Task Delete()
        {

        }

        public Models.Task GetAll()
        {

        }
    }
}
