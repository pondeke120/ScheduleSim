using ScheduleSim.Entities.Models;
using ScheduleSim.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Service
{
    public class ProjectSettingsAccessService : IProjectSettingsAccessService
    {
        private IFunctionRepository functionRepository;
        private IHolidayRepository holidayRepository;
        private IPrjSettingsRepository prjSettingsRepository;
        private IProcessRepository processRepository;
        private IWeekdayRepository weekdayRepository;

        public ProjectSettingsAccessService(
            IPrjSettingsRepository prjSettingsRepository,
            IProcessRepository processRepository,
            IFunctionRepository functionRepository,
            IHolidayRepository holidayRepository,
            IWeekdayRepository weekdayRepository)
        {
            this.prjSettingsRepository = prjSettingsRepository;
            this.processRepository = processRepository;
            this.functionRepository = functionRepository;
            this.holidayRepository = holidayRepository;
            this.weekdayRepository = weekdayRepository;
        }

        public void GetAllSetting(
            ref DateTime? startDate, 
            ref DateTime? endDate, 
            ref IEnumerable<Process> processes, 
            ref IEnumerable<Function> functions, 
            ref IEnumerable<Holiday> holidays, 
            ref IEnumerable<WeekDay> restDays)
        {
            var settings = this.prjSettingsRepository.FindRecently();
            startDate = settings.StartDate;
            endDate = settings.EndDate;
            processes = this.processRepository.Find().ToArray();
            functions = this.functionRepository.Find().ToArray();
            holidays = this.holidayRepository.Find().ToArray();
            restDays = this.weekdayRepository.Find().ToArray();
        }

        public void Update(
            DateTime? startDate, 
            DateTime? endDate, 
            IEnumerable<Process> processes, 
            IEnumerable<Function> functions, 
            IEnumerable<Holiday> holidays, 
            IEnumerable<WeekDay> restDays)
        {
            this.prjSettingsRepository.RemoveAll();
            this.prjSettingsRepository.Insert(new Entities.Models.PrjSettings() { StartDate = startDate, EndDate = endDate });
            this.processRepository.RemoveAll();
            this.processRepository.Insert(processes);
            this.functionRepository.RemoveAll();
            this.functionRepository.Insert(functions);
            this.holidayRepository.RemoveAll();
            this.holidayRepository.Insert(holidays);
            this.weekdayRepository.RemoveAll();
            this.weekdayRepository.Insert(restDays);
        }
    }
}
