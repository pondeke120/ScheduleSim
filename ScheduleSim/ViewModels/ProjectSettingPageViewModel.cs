using AutoMapper;
using Prism.Mvvm;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Core.Utility;
using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public ICommand ProcessChangeCommand { get; private set; }
        public ICommand FunctionChangeCommand { get; private set; }
        public ICommand PeriodChangeCommand { get; private set; }
        private IMapper mapper;

        public ProjectSettingPageViewModel(
            AppContext appContext,
            ICommand processChangeCommand,
            ICommand functionChangeCommand,
            ICommand periodChangeCommand,
            IMapper mapper,
            IIDGenerator holidayIdGen)
        {
            //this.ProjectStartDate = new DateTime(2018, 10, 2);
            //this.ProjectEndDate = new DateTime(2018, 10, 30);
            //this.ProcessNames = new string[20].Select(x => new ProjectSettingPageProcessItemViewModel() { Id = processIdGen.CreateNewId(), Name = x }).ToList();
            //this.ProcessNames[0].Name = "testP1";
            //this.ProcessNames[1].Name = "testP2";
            //this.FunctionNames = new string[20].Select(x => new ProjectSettingPageFunctionItemViewModel() { Id = functionIdGen.CreateNewId(), Name = x }).ToList();
            //this.FunctionNames[0].Name = "testF1";
            //this.FunctionNames[1].Name = "testF2";
            this.Holidays = new string[20].Select(x => new ProjectSettingPageHolidayItemViewModel() { Id = holidayIdGen.CreateNewId(), Date = null }).ToList();
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

            this.ProcessChangeCommand = processChangeCommand;
            this.FunctionChangeCommand = functionChangeCommand;
            this.PeriodChangeCommand = periodChangeCommand;
            this.mapper = mapper;
            appContext.Processes.CollectionChanged += Processes_CollectionChanged;
            appContext.Functions.CollectionChanged += Functions_CollectionChanged;
            appContext.PrjSettings.PropertyChanged += PrjSettings_PropertyChanged;
        }

        /// <summary>
        /// プロジェクト設定(日付)の変更イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrjSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var settings = sender as PrjSettings;
            if (e.PropertyName.Equals(nameof(settings.StartDate)))
            {
                this.ProjectStartDate = settings.StartDate;
            }
            else if (e.PropertyName.Equals(nameof(settings.EndDate)))
            {
                this.ProjectEndDate = settings.EndDate;
            }
        }

        /// <summary>
        /// 機能名変更時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Functions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<Function>;

            if (this.FunctionNames == null || this.FunctionNames.Count == 0)
            {
                this.FunctionNames = this.mapper.Map<List<ProjectSettingPageFunctionItemViewModel>>(sender);
            }
            else
            {
                var current = this.FunctionNames.ToList();
                var updateIds = collection.Select(x => x.FunctionCd as int?).ToArray();
                var replaceItems = current.Where(x => updateIds.Contains(x.Id))
                                                    .Select(x => new { Old = x, New = collection.First(a => a.FunctionCd == x.Id) })
                                                    .ToArray();
                var deleteItem = current.Where(x => updateIds.Contains(x.Id) == false).ToArray();

                foreach (var item in replaceItems)
                {
                    var index = current.IndexOf(item.Old);
                    current.RemoveAt(index);
                    current.Insert(index, this.mapper.Map<ProjectSettingPageFunctionItemViewModel>(item.New));
                }
                foreach (var item in deleteItem)
                {
                    item.Name = null;
                }

                this.FunctionNames = current;
            }
        }

        /// <summary>
        /// 工程変更時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Processes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<Process>;

            if (this.ProcessNames == null || this.ProcessNames.Count == 0)
            {
                this.ProcessNames = this.mapper.Map<List<ProjectSettingPageProcessItemViewModel>>(sender);
            }
            else
            {
                var current = this.ProcessNames.ToList();
                var updateIds = collection.Select(x => x.ProcessCd as int?).ToArray();
                var replaceItems = current.Where(x => updateIds.Contains(x.Id))
                                                    .Select(x => new { Old = x, New = collection.First(a => a.ProcessCd == x.Id)})
                                                    .ToArray();
                var deleteItem = current.Where(x => updateIds.Contains(x.Id) == false).ToArray();

                foreach (var item in replaceItems)
                {
                    var index = current.IndexOf(item.Old);
                    current.RemoveAt(index);
                    current.Insert(index, this.mapper.Map<ProjectSettingPageProcessItemViewModel>(item.New));
                }
                foreach (var item in deleteItem)
                {
                    item.Name = null;
                }

                this.ProcessNames = current;
            }
        }
    }
}
