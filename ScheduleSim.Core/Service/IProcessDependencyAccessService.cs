using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Service
{
    public interface IProcessDependencyAccessService
    {
        void Update(IEnumerable<ProcessDependency> dependencies);
        IEnumerable<ProcessDependency> GetAllDependencies();
    }
}
