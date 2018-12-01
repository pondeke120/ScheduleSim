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
    public class FunctionDependencyPageViewModel : BindableBase
    {
        private AppContext appContext;
        private IMapper mapper;
        public ICommand AddDependencyCommand { get; private set; }
        public ICommand DeleteDependencyCommand { get; private set; }
        public ICommand SrcFunctionChangeCommand { get; private set; }
        public ICommand DstFunctionChangeCommand { get; private set; }
        public ICommand DependencyTypeChangeCommand { get; private set; }

        private ObservableCollection<FunctionDependencyPageDependencyItemViewModel> _dependencies;
        public ObservableCollection<FunctionDependencyPageDependencyItemViewModel> Dependencies
        {
            get { return _dependencies; }
            set { SetProperty(ref _dependencies, value); }
        }

        private ObservableCollection<FunctionDependencyPageFunctionItemViewModel> _functionSource;
        public ObservableCollection<FunctionDependencyPageFunctionItemViewModel> FunctionSource
        {
            get { return _functionSource; }
            set { SetProperty(ref _functionSource, value); }
        }

        private ObservableCollection<FunctionDependencyPageDependencyTypeItemViewModel> _dependencyTypeSource;
        public ObservableCollection<FunctionDependencyPageDependencyTypeItemViewModel> DependencyTypeSource
        {
            get { return _dependencyTypeSource; }
            set { SetProperty(ref _dependencyTypeSource, value); }
        }

        public FunctionDependencyPageViewModel(
            AppContext appContext,
            IMapper mapper,
            ICommand addDependencyCommand,
            ICommand deleteDependencyCommand,
            ICommand srcFunctionChangeCommand,
            ICommand dstFunctionChangeCommand,
            ICommand dependencyTypeChangeCommand)
        {
            this.appContext = appContext;
            this.mapper = mapper;
            AddDependencyCommand = addDependencyCommand;
            DeleteDependencyCommand = deleteDependencyCommand;
            SrcFunctionChangeCommand = srcFunctionChangeCommand;
            DstFunctionChangeCommand = dstFunctionChangeCommand;
            DependencyTypeChangeCommand = dependencyTypeChangeCommand;

            //Dependencies = new List<FunctionDependencyPageDependencyItemViewModel>()
            //{
            //    new FunctionDependencyPageDependencyItemViewModel() { SrcFunctionId = 1, DstFunctionId = 2, DependencyType = Entities.Enum.DependencyTypes.CompleteDependency }
            //};


            appContext.Functions.CollectionChanged += Functions_CollectionChanged;
            appContext.FunctionDependencies.CollectionChanged += FunctionDependencies_CollectionChanged;
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

            this.DependencyTypeSource = new ObservableCollection<FunctionDependencyPageDependencyTypeItemViewModel>(
                                            new[] { new FunctionDependencyPageDependencyTypeItemViewModel() { DataContext = null } }
                                            .Concat(this.mapper.Map<ObservableCollection<FunctionDependencyPageDependencyTypeItemViewModel>>(sender))
                                        );
        }

        /// <summary>
        /// 機能依存関係変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FunctionDependencies_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var dependencies = sender as ObservableCollection<FunctionDependency>;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset
                || e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                this.Dependencies = this.mapper.Map<ObservableCollection<FunctionDependencyPageDependencyItemViewModel>>(dependencies);
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

            this.FunctionSource = new ObservableCollection<FunctionDependencyPageFunctionItemViewModel>(
                                    new[] { new FunctionDependencyPageFunctionItemViewModel() { DataContext = null } }
                                    .Concat(this.mapper.Map<ObservableCollection<FunctionDependencyPageFunctionItemViewModel>>(sender))
                                );
        }
    }
}
