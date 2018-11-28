using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleSim.Core.Service
{
    public interface ITaskAccessService
    {
        void Update(IEnumerable<Task> tasks);
        IEnumerable<Task> GetAllTasks();
        int GetCurrentIndex();
    }
}
