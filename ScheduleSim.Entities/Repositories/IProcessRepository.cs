using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Repositories
{
    public interface IProcessRepository
    {
        void Insert(IEnumerable<Process> processes);
        IEnumerable<Process> Find();
        void RemoveAll();
    }
}
