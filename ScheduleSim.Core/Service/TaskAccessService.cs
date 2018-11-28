using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScheduleSim.Entities.Models;
using ScheduleSim.Entities.Repositories;

namespace ScheduleSim.Core.Service
{
    public class TaskAccessService : ITaskAccessService
    {
        private ITaskRepository taskRepository;

        public TaskAccessService(
            ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        public IEnumerable<Task> GetAllTasks()
        {
            return
                this.taskRepository.Find();
        }

        public void Update(IEnumerable<Task> tasks)
        {
            this.taskRepository.RemoveAll();
            this.taskRepository.Insert(tasks);
        }

        public int GetCurrentIndex()
        {
            return
                this.taskRepository.GetCurrentIndex();
        }
    }
}
