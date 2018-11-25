using AutoMapper;
using Prism.Mvvm;
using ScheduleSim.Core.Contexts;
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
    public class WbsPageViewModel : BindableBase
    {
        public ICommand DeleteTaskCommand { get; private set; }
        private IMapper mapper;

        private List<WbsPageTaskItemViewModel> _tasks;
        public List<WbsPageTaskItemViewModel> Tasks
        {
            get { return _tasks; }
            set { SetProperty(ref _tasks, value); }
        }

        private List<WbsPageProcessItemViewModel> _processSource;
        public List<WbsPageProcessItemViewModel> ProcessSource
        {
            get { return _processSource; }
            set { SetProperty(ref _processSource, value); }
        }

        private List<WbsPageFunctionItemViewModel> _functionSource;
        public List<WbsPageFunctionItemViewModel> FunctionSource
        {
            get { return _functionSource; }
            set { SetProperty(ref _functionSource, value); }
        }

        public WbsPageViewModel(
            AppContext appContext,
            IMapper mapper,
            ICommand deleteTaskCommand)
        {
            DeleteTaskCommand = deleteTaskCommand;
            this.mapper = mapper;

            Tasks = new List<WbsPageTaskItemViewModel>()
            {
                new WbsPageTaskItemViewModel() { No = 1, ProcessId = 1, FunctionId = 1, TaskName = "task1", PV = 2.0, StartDate = new DateTime(2018, 10, 2), EndDate = new DateTime(2018, 10, 30), AssignMemberId = 1 },
                new WbsPageTaskItemViewModel() { No = 2, ProcessId = 1, FunctionId = 1, TaskName = "task2", PV = 8.0 },
                new WbsPageTaskItemViewModel() { No = 3, ProcessId = 1, FunctionId = 1, TaskName = "task3" }
            };

            appContext.Processes.CollectionChanged += Processes_CollectionChanged;
            appContext.Functions.CollectionChanged += Functions_CollectionChanged;
        }

        private void Functions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<Function>;

            this.FunctionSource = this.mapper.Map<List<WbsPageFunctionItemViewModel>>(sender);
        }

        /// <summary>
        /// 工程変更時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Processes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<Process>;

            this.ProcessSource = this.mapper.Map<List<WbsPageProcessItemViewModel>>(sender);
        }
    }
}
