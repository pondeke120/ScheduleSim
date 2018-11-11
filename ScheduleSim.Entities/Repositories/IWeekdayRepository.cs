using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Repositories
{
    public interface IWeekdayRepository
    {
        void Insert(IEnumerable<WeekDay> weekDays);
        void Update(IEnumerable<WeekDay> weekDays);
        IEnumerable<WeekDay> Find();
        void RemoveAll();
    }
}
