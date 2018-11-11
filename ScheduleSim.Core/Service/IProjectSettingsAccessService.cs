using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Service
{
    public interface IProjectSettingsAccessService
    {
        void Update(
            DateTime? startDate,
            DateTime? endDate,
            IEnumerable<Process> processes,
            IEnumerable<Function> functions,
            IEnumerable<Holiday> holidays,
            IEnumerable<WeekDay> restDays);

        void GetAllSetting(
            ref DateTime? startDate,
            ref DateTime? endDate,
            ref IEnumerable<Process> processes,
            ref IEnumerable<Function> functions,
            ref IEnumerable<Holiday> holidays,
            ref IEnumerable<WeekDay> restDays);
    }
}
