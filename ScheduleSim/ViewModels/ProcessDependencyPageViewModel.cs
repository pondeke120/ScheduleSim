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
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScheduleSim.ViewModels
{
    public class ProcessDependencyPageViewModel : BindableBase
    {
        private AppContext appContext;
        private IMapper mapper;
        public ICommand AddDependencyCommand { get; private set; }
        public ICommand DeleteDependencyCommand { get; private set; }
        public ICommand SrcProcessChangeCommand { get; private set; }
        public ICommand DstProcessChangeCommand { get; private set; }
        public ICommand DependencyTypeChangeCommand { get; private set; }
        public ICommand InsertDependencyCommand { get; private set; }

        private ObservableCollection<ProcessDependencyPageDependencyItemViewModel> _dependencies;
        public ObservableCollection<ProcessDependencyPageDependencyItemViewModel> Dependencies
        {
            get { return _dependencies; }
            set { SetProperty(ref _dependencies, value); }
        }

        private ObservableCollection<ProcessDependencyPageProcessItemViewModel> _processSource;
        public ObservableCollection<ProcessDependencyPageProcessItemViewModel> ProcessSource
        {
            get { return _processSource; }
            set { SetProperty(ref _processSource, value); }
        }

        private ObservableCollection<ProcessDependencyPageDependencyTypeItemViewModel> _dependencyTypeSource;
        public ObservableCollection<ProcessDependencyPageDependencyTypeItemViewModel> DependencyTypeSource
        {
            get { return _dependencyTypeSource; }
            set { SetProperty(ref _dependencyTypeSource, value); }
        }

        private IList _selectedDependencies;
        public IList SelectedDependencies
        {
            get { return _selectedDependencies; }
            set { SetProperty(ref _selectedDependencies, value); }
        }

        public ProcessDependencyPageViewModel(
            AppContext appContext,
            IMapper mapper,
            ICommand addDependencyCommand,
            ICommand deleteDependencyCommand,
            ICommand srcProcessChangeCommand,
            ICommand dstProcessChangeCommand,
            ICommand dependencyTypeChangeCommand,
            ICommand insertDependencyCommand)
        {
            this.appContext = appContext;
            this.mapper = mapper;
            AddDependencyCommand = addDependencyCommand;
            DeleteDependencyCommand = deleteDependencyCommand;
            SrcProcessChangeCommand = srcProcessChangeCommand;
            DstProcessChangeCommand = dstProcessChangeCommand;
            DependencyTypeChangeCommand = dependencyTypeChangeCommand;
            InsertDependencyCommand = insertDependencyCommand;

            //Dependencies = new ObservableCollection<ProcessDependencyPageDependencyItemViewModel>()
            //{
            //    new ProcessDependencyPageDependencyItemViewModel() { SrcProcessId = 1, DstProcessId = 2, DependencyType = Entities.Enum.DependencyTypes.StartDependency },
            //};

            appContext.Processes.CollectionChanged += Processes_CollectionChanged;
            appContext.ProcessDependencies.CollectionChanged += ProcessDependencies_CollectionChanged;
            appContext.DependencyTypes.CollectionChanged += DependencyTypes_CollectionChanged;
        }

        /// <summary>
        /// 依存関係種類変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DependencyTypes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<DependencyType>;

            this.DependencyTypeSource = new ObservableCollection<ProcessDependencyPageDependencyTypeItemViewModel>(
                                            new[] { new ProcessDependencyPageDependencyTypeItemViewModel() { DataContext = null } }
                                            .Concat(this.mapper.Map<ObservableCollection<ProcessDependencyPageDependencyTypeItemViewModel>>(sender))
                                        );
        }

        /// <summary>
        /// 工程依存関係変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessDependencies_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var dependencies = sender as ObservableCollection<ProcessDependency>;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset
                || e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                this.Dependencies = this.mapper.Map<ObservableCollection<ProcessDependencyPageDependencyItemViewModel>>(dependencies);
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

            this.ProcessSource = new ObservableCollection<ProcessDependencyPageProcessItemViewModel>(
                                    new [] { new ProcessDependencyPageProcessItemViewModel() { DataContext = null } }
                                    .Concat(this.mapper.Map<ObservableCollection<ProcessDependencyPageProcessItemViewModel>>(sender))
                                );
        }
    }
}
