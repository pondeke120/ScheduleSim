using AutoMapper;
using Microsoft.Win32;
using ScheduleSim.Core.BusinessLogics.WPF.Menu;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Core.Dispatcher;
using ScheduleSim.Core.IO.WPF.Menu;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ScheduleSim.Core.Extensions;
using ScheduleSim.Core.Utility;

namespace ScheduleSim.Commands.Menu
{
    public class OpenFileCommand : ICommand
    {
        private AppContext appContext;
        private IOpenFileBusinessLogic openFileBusinessLogic;
        private ProcessDependencyPageViewModel processDependencyPageViewModel;
        private FunctionDependencyPageViewModel functionDependencyPageViewModel;
        private WbsPageViewModel wbsPageViewModel;
        private PertPageViewModel pertPageViewModel;
        private ShellViewModel shellViewModel;
        private IIDGenerator memberIdGen;
        private IMapper mapper;
        private IDispatcher dispatcher;

        public event EventHandler CanExecuteChanged;

        public OpenFileCommand(
            AppContext appContext,
            IOpenFileBusinessLogic openFileBusinessLogic,
            ProcessDependencyPageViewModel processDependencyPageViewModel,
            FunctionDependencyPageViewModel functionDependencyPageViewModel,
            WbsPageViewModel wbsPageViewModel,
            PertPageViewModel pertPageViewModel,
            ShellViewModel shellViewModel,
            IIDGenerator memberIdGen,
            IMapper mapper,
            IDispatcher dispatcher)
        {
            this.appContext = appContext;
            this.openFileBusinessLogic = openFileBusinessLogic;
            this.processDependencyPageViewModel = processDependencyPageViewModel;
            this.functionDependencyPageViewModel = functionDependencyPageViewModel;
            this.wbsPageViewModel = wbsPageViewModel;
            this.pertPageViewModel = pertPageViewModel;
            this.shellViewModel = shellViewModel;
            this.memberIdGen = memberIdGen;
            this.mapper = mapper;
            this.dispatcher = dispatcher;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == false)
            {
                return;
            }

            var input = new OpenFileInput();

            input.FilePath = ofd.FileName;
            var projectDbFileBak = this.appContext.ProjectDbFile;
            this.appContext.ProjectDbFile = input.FilePath;
            this.appContext.ProjectFolder = Path.GetDirectoryName(input.FilePath);

            var output = this.dispatcher.Dispatch(this.openFileBusinessLogic, input);

            if (output?.IsError ?? true)
            {
                this.appContext.ProjectDbFile = projectDbFileBak;
                this.appContext.ProjectFolder = Path.GetDirectoryName(projectDbFileBak);
                MessageBox.Show("ファイル読み込みに失敗しました。");
                return;
            }

            this.appContext.IsSaved = true;
            this.appContext.ProjectDbFile = output.FilePath;
            this.appContext.PrjSettings.StartDate = output.StartDate;
            this.appContext.PrjSettings.EndDate = output.EndDate;

            this.appContext.Processes.Clear();
            this.appContext.Processes.AddRange(output.Processes);

            this.appContext.Functions.Clear();
            this.appContext.Functions.AddRange(output.Functions);

            this.appContext.Holidays.Clear();
            this.appContext.Holidays.AddRange(output.Holidays);
            
            this.appContext.WeekDays.Clear();
            this.appContext.WeekDays.AddRange(output.RestDays);

            this.memberIdGen.SetCurrentIndex(output.MaxMemberId + 1);
            this.appContext.Members.Clear();
            this.appContext.Members.AddRange(output.Members);

            this.wbsPageViewModel.Tasks = this.mapper.Map<List<WbsPageTaskItemViewModel>>(output.Tasks);

            this.processDependencyPageViewModel.Dependencies = this.mapper.Map<List<ProcessDependencyPageDependencyItemViewModel>>(output.ProcessDependencies);
            
            this.functionDependencyPageViewModel.Dependencies = this.mapper.Map<List<FunctionDependencyPageDependencyItemViewModel>>(output.FunctionDependencies);
            
            this.pertPageViewModel.Edges = this.mapper.Map<List<PertPageEdgeItemViewModel>>(output.Edges);

            this.shellViewModel.ProjectPath = output?.FilePath ?? projectDbFileBak;
        }
    }
}
