using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Repositories
{
    public interface IPertRepository
    {
        void RemoveAll();
        void Insert(IEnumerable<Pert> edges);
        IEnumerable<Pert> Find();
        int GetCurrentIndex();
    }
}
