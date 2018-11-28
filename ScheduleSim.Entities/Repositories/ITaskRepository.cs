using ScheduleSim.Entities.Models;
using System.Collections.Generic;

namespace ScheduleSim.Entities.Repositories
{
    public interface ITaskRepository
    {
        void Insert(IEnumerable<Task> tasks);
        IEnumerable<Task> Find();
        void RemoveAll();
        int GetCurrentIndex();
    }
}
