using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Repositories
{
    public interface IHolidayRepository
    {
        void Insert(IEnumerable<Holiday> holidays);
        IEnumerable<Holiday> Find();
        void RemoveAll();
    }
}
