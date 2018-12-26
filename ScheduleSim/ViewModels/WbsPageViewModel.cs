using AutoMapper;
using Prism.Mvvm;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ScheduleSim.Core.Extensions;
using System.Collections;

namespace ScheduleSim.ViewModels
{
    public class WbsPageViewModel : BindableBase
    {
        public ICommand AddTaskCommand { get; private set; }
        public ICommand DeleteTaskCommand { get; private set; }
        public ICommand ProcessChangeCommand { get; private set; }
        public ICommand FunctionChangeCommand { get; private set; }
        public ICommand TaskNameChangeCommand { get; private set; }
        public ICommand PlanValueChangeCommand { get; private set; }
        public ICommand StartDateChangeCommand { get; private set; }
        public ICommand EndDateChangeCommand { get; private set; }
        public ICommand AssignMemberChangeCommand { get; private set; }
        public ICommand InsertTaskCommand { get; private set; }
        private IMapper mapper;

        private ObservableCollection<WbsPageTaskItemViewModel> _tasks;
        public ObservableCollection<WbsPageTaskItemViewModel> Tasks
        {
            get { return _tasks; }
            set { SetProperty(ref _tasks, value); }
        }

        private ObservableCollection<WbsPageProcessItemViewModel> _processSource;
        public ObservableCollection<WbsPageProcessItemViewModel> ProcessSource
        {
            get { return _processSource; }
            set { SetProperty(ref _processSource, value); }
        }

        private ObservableCollection<WbsPageFunctionItemViewModel> _functionSource;
        public ObservableCollection<WbsPageFunctionItemViewModel> FunctionSource
        {
            get { return _functionSource; }
            set { SetProperty(ref _functionSource, value); }
        }

        private ObservableCollection<WbsPageMemberItemViewModel> _memberSource;
        public ObservableCollection<WbsPageMemberItemViewModel> MemberSource
        {
            get { return _memberSource; }
            set { SetProperty(ref _memberSource, value); }
        }

        private IList _selectedTasks;
        public IList SelectedTasks
        {
            get { return _selectedTasks; }
            set { SetProperty(ref _selectedTasks, value); }
        }

        public WbsPageViewModel(
            AppContext appContext,
            IMapper mapper,
            ICommand addTaskCommand,
            ICommand deleteTaskCommand,
            ICommand processChangeCommand,
            ICommand functionChangeCommand,
            ICommand taskNameChangeCommand,
            ICommand planValueChangeCommand,
            ICommand startDateChangeCommand,
            ICommand endDateChangeCommand,
            ICommand assignMemberChangeCommand,
            ICommand insertTaskCommand)
        {
            AddTaskCommand = addTaskCommand;
            DeleteTaskCommand = deleteTaskCommand;
            ProcessChangeCommand = processChangeCommand;
            FunctionChangeCommand = functionChangeCommand;
            TaskNameChangeCommand = taskNameChangeCommand;
            PlanValueChangeCommand = planValueChangeCommand;
            StartDateChangeCommand = startDateChangeCommand;
            EndDateChangeCommand = endDateChangeCommand;
            AssignMemberChangeCommand = assignMemberChangeCommand;
            InsertTaskCommand = insertTaskCommand;
            this.mapper = mapper;

            //Tasks = new List<WbsPageTaskItemViewModel>()
            //{
            //    new WbsPageTaskItemViewModel() { No = 1, ProcessId = 1, FunctionId = 1, TaskName = "task1", PV = 2.0, StartDate = new DateTime(2018, 10, 2), EndDate = new DateTime(2018, 10, 30), AssignMemberId = 1 },
            //    new WbsPageTaskItemViewModel() { No = 2, ProcessId = 1, FunctionId = 1, TaskName = "task2", PV = 8.0 },
            //    new WbsPageTaskItemViewModel() { No = 3, ProcessId = 1, FunctionId = 1, TaskName = "task3" }
            //};

            appContext.Processes.CollectionChanged += Processes_CollectionChanged;
            appContext.Functions.CollectionChanged += Functions_CollectionChanged;
            appContext.Members.CollectionChanged += Members_CollectionChanged;
            appContext.Tasks.CollectionChanged += Tasks_CollectionChanged;
        }

        /// <summary>
        /// タスクの変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var tasks = sender as ObservableCollection<Task>;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset
                || e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                this.Tasks = this.mapper.Map<ObservableCollection<WbsPageTaskItemViewModel>>(tasks);
            }
        }

        /// <summary>
        /// 要員変更時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Members_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<Member>;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
                this.MemberSource = this.mapper.Map<ObservableCollection<WbsPageMemberItemViewModel>>(sender);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                var newItems = this.mapper.Map<List<WbsPageMemberItemViewModel>>(e.NewItems);
                this.MemberSource.AddRange(newItems);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var oldItemIds = e.OldItems.Cast<Member>().Select(x => x.MemberCd as int?).ToArray();
                var oldItems = this.MemberSource.Where(x => oldItemIds.Contains(x.MemberCd)).ToArray();
                this.MemberSource.RemoveRange(oldItems);
            }
        }

        /// <summary>
        /// 機能変更時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Functions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<Function>;

            this.FunctionSource = this.mapper.Map<ObservableCollection<WbsPageFunctionItemViewModel>>(sender);
        }

        /// <summary>
        /// 工程変更時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Processes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<Process>;

            this.ProcessSource = this.mapper.Map<ObservableCollection<WbsPageProcessItemViewModel>>(sender);
        }
    }
}
