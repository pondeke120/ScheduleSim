using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Repositories
{
    public interface IFunctionRepository
    {
        void Insert(IEnumerable<Function> functions);
        IEnumerable<Function> Find();
        void RemoveAll();
    }
}
