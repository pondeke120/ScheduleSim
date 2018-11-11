using Prism.Mvvm;
using ScheduleSim.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class ProjectSettingPageViewModel : BindableBase
    {
        private List<ProjectSettingPageProcessItemViewModel> _processNames;
        public List<ProjectSettingPageProcessItemViewModel> ProcessNames
        {
            get { return _processNames; }
            set { SetProperty(ref _processNames, value); }
        }

        private List<ProjectSettingPageFunctionItemViewModel> _functionNames;
        public List<ProjectSettingPageFunctionItemViewModel> FunctionNames
        {
            get { return _functionNames; }
            set { SetProperty(ref _functionNames, value); }
        }

        private List<ProjectSettingPageHolidayItemViewModel> _holidays;
        public List<ProjectSettingPageHolidayItemViewModel> Holidays
        {
            get { return _holidays; }
            set { SetProperty(ref _holidays, value); }
        }

        private List<ProjectSettingPageWeekdayItemViewModel> _weekdays;
        public List<ProjectSettingPageWeekdayItemViewModel> Weekdays
        {
            get { return _weekdays; }
            set { SetProperty(ref _weekdays, value); }
        }

        private DateTime? _projectStartDate;
        public DateTime? ProjectStartDate
        {
            get { return _projectStartDate; }
            set { SetProperty(ref _projectStartDate, value); }
        }

        private DateTime? _projectEndDate;
        public DateTime? ProjectEndDate
        {
            get { return _projectEndDate; }
            set { SetProperty(ref _projectEndDate, value); }
        }

        public ProjectSettingPageViewModel(
            IIDGenerator processIdGen,
            IIDGenerator functionIdGen,
            IIDGenerator holidayIdGen)
        {
            this.ProjectStartDate = new DateTime(2018, 10, 2);
            this.ProjectEndDate = new DateTime(2018, 10, 30);
            this.ProcessNames = new string[20].Select(x => new ProjectSettingPageProcessItemViewModel() { Id = processIdGen.CreateNewId(), Name = x}).ToList();
            this.ProcessNames[0].Name = "testP1";
            this.ProcessNames[1].Name = "testP2";
            this.FunctionNames = new string[20].Select(x => new ProjectSettingPageFunctionItemViewModel() { Id = functionIdGen.CreateNewId(), Name = x }).ToList();
            this.FunctionNames[0].Name = "testF1";
            this.FunctionNames[1].Name = "testF2";
            this.Holidays = new string[20].Select(x => new ProjectSettingPageHolidayItemViewModel() { Id = holidayIdGen.CreateNewId(), Date = null}).ToList();
            this.Holidays[0].Date = new DateTime(2018, 2, 28);

            this.Weekdays = new List<ProjectSettingPageWeekdayItemViewModel>()
            {
                new ProjectSettingPageWeekdayItemViewModel() { IsCheck = true, DayOfWeek = DayOfWeek.Monday },
                new ProjectSettingPageWeekdayItemViewModel() { IsCheck = false, DayOfWeek = DayOfWeek.Tuesday },
                new ProjectSettingPageWeekdayItemViewModel() { IsCheck = true, DayOfWeek = DayOfWeek.Wednesday },
                new ProjectSettingPageWeekdayItemViewModel() { IsCheck = false, DayOfWeek = DayOfWeek.Thursday },
                new ProjectSettingPageWeekdayItemViewModel() { IsCheck = true, DayOfWeek = DayOfWeek.Friday },
                new ProjectSettingPageWeekdayItemViewModel() { IsCheck = false, DayOfWeek = DayOfWeek.Saturday },
                new ProjectSettingPageWeekdayItemViewModel() { IsCheck = true, DayOfWeek = DayOfWeek.Sunday },
            };
        }
    }
}
