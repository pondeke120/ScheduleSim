using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Repositories
{
    public interface IDependencyTypeRepository
    {
        IEnumerable<DependencyType> Find();
        void Insert(IEnumerable<DependencyType> dependencyTypes);
        void RemoveAll();
    }
}
