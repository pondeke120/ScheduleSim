using AutoMapper;
using Prism.Mvvm;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Entities.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ScheduleSim.ViewModels
{
    public class PertPageViewModel : BindableBase
    {
        public ICommand AddEdgeCommand { get; private set; }
        public ICommand DeleteEdgeCommand { get; private set; }
        public ICommand ProcessChangeCommand { get; private set; }
        public ICommand FunctionChangeCommand { get; private set; }
        public ICommand TaskChangeCommand { get; private set; }
        public ICommand SrcNodeChangeCommand { get; private set; }
        public ICommand DstNodeChangeCommand { get; private set; }
        public ICommand UpdateCalcValuesCommand { get; private set; }
        public ICommand TaskSelectionSourceFilterCommand { get; private set; }
        public ICommand ImportFromWbsCommand { get; private set; }
        public ICommand InsertEdgeCommand { get; private set; }
        public ICommand CheckDependencyCommand { get; private set; }
        private IMapper mapper;

        private ObservableCollection<PertPageEdgeItemViewModel> _edges;
        public ObservableCollection<PertPageEdgeItemViewModel> Edges
        {
            get { return _edges; }
            set { SetProperty(ref _edges, value); }
        }
        
        private ObservableCollection<PertPageTaskItemViewModel> _taskSource;
        public ObservableCollection<PertPageTaskItemViewModel> TaskSource
        {
            get { return _taskSource; }
            set { SetProperty(ref _taskSource, value); }
        }

        private ObservableCollection<PertPageProcessItemViewModel> _processSource;
        public ObservableCollection<PertPageProcessItemViewModel> ProcessSource
        {
            get { return _processSource; }
            set { SetProperty(ref _processSource, value); }
        }

        private ObservableCollection<PertPageFunctionItemViewModel> _functionSource;
        public ObservableCollection<PertPageFunctionItemViewModel> FunctionSource
        {
            get { return _functionSource; }
            set { SetProperty(ref _functionSource, value); }
        }

        private IList _selectedEdges;
        public IList SelectedEdges
        {
            get { return _selectedEdges; }
            set { SetProperty(ref _selectedEdges, value); }
        }

        private AppContext appContext;

        public PertPageViewModel(
            AppContext appContext,
            IMapper mapper,
            ICommand addEdgeCommand,
            ICommand deleteEdgeCommand,
            ICommand processChangeCommand,
            ICommand functionChangeCommand,
            ICommand taskChangeCommand,
            ICommand srcNodeChangeCommand,
            ICommand dstNodeChangeCommand,
            ICommand updateCalcValuesCommand,
            ICommand taskSelectionSourceFilterCommand,
            ICommand importFromWbsCommand,
            ICommand insertEdgeCommand,
            ICommand checkDependencyCommand)
        {
            this.AddEdgeCommand = addEdgeCommand;
            this.DeleteEdgeCommand = deleteEdgeCommand;
            this.ProcessChangeCommand = processChangeCommand;
            this.FunctionChangeCommand = functionChangeCommand;
            this.TaskChangeCommand = taskChangeCommand;
            this.SrcNodeChangeCommand = srcNodeChangeCommand;
            this.DstNodeChangeCommand = dstNodeChangeCommand;
            this.UpdateCalcValuesCommand = updateCalcValuesCommand;
            this.TaskSelectionSourceFilterCommand = taskSelectionSourceFilterCommand;
            this.ImportFromWbsCommand = importFromWbsCommand;
            this.InsertEdgeCommand = insertEdgeCommand;
            this.CheckDependencyCommand = checkDependencyCommand;
            this.mapper = mapper;
            this.appContext = appContext;

            //Edges = new ObservableCollection<PertPageEdgeItemViewModel>()
            //{
            //    new PertPageEdgeItemViewModel() { INode = 1, JNode = 2, TaskId = 0 },
            //    new PertPageEdgeItemViewModel() { INode = 1, JNode = 3, TaskId = 1 },
            //};


            appContext.Processes.CollectionChanged += Processes_CollectionChanged;
            appContext.Functions.CollectionChanged += Functions_CollectionChanged;
            appContext.Tasks.CollectionChanged += Task_CollectionChanged;
            appContext.PertEdges.CollectionChanged += PertEdges_CollectionChanged;
        }

        /// <summary>
        /// Pert変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PertEdges_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var edges = sender as ObservableCollection<Pert>;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset
                || e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                this.Edges = this.mapper.Map<ObservableCollection<PertPageEdgeItemViewModel>>(edges);

                if (Edges.Count > 0
                    && e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
                {
                    this.UpdateCalcValuesCommand.Execute(this);
                }
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

            this.FunctionSource = new ObservableCollection<PertPageFunctionItemViewModel>(
                                    new[] { new PertPageFunctionItemViewModel() { DataContext = null } }
                                    .Concat(this.mapper.Map<ObservableCollection<PertPageFunctionItemViewModel>>(sender))
                                );
        }

        /// <summary>
        /// 工程変更時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Processes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<Process>;
            
            this.ProcessSource = new ObservableCollection<PertPageProcessItemViewModel>(
                                    new[] { new PertPageProcessItemViewModel() { DataContext = null } }
                                    .Concat(this.mapper.Map<ObservableCollection<PertPageProcessItemViewModel>>(sender))
                                );
        }

        /// <summary>
        /// タスク変更時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Task_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<Task>;

            this.TaskSource = new ObservableCollection<PertPageTaskItemViewModel>(
                                    new[] { new PertPageTaskItemViewModel() { DataContext = null } }
                                    .Concat(this.mapper.Map<ObservableCollection<PertPageTaskItemViewModel>>(sender))
                                );
        }
    }
}
