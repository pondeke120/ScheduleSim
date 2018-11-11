using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Repositories
{
    public interface IProcessDependencyRepository
    {
        void Insert(IEnumerable<ProcessDependency> dependencies);
        IEnumerable<ProcessDependency> Find();
        void RemoveAll();
    }
}
