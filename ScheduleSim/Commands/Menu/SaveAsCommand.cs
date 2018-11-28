using AutoMapper;
using Microsoft.Win32;
using ScheduleSim.Core.BusinessLogics.WPF.Menu;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Core.Dispatcher;
using ScheduleSim.Core.IO.WPF.Menu;
using ScheduleSim.Entities.Models;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ScheduleSim.Commands.Menu
{
    public class SaveAsCommand : ICommand
    {
        private ISaveAsBusinessLogic businessLogic;
        private AppContext appContext;
        private ProjectSettingPageViewModel projectSettingPageViewModel;
        private ShellViewModel shellViewModel;
        private MemberPageViewModel memberPageViewModel;
        private IMapper mapper;
        private WbsPageViewModel wbsPageViewModel;
        private ProcessDependencyPageViewModel processDependencyPageViewModel;
        private FunctionDependencyPageViewModel functionDependencyPageViewModel;
        private PertPageViewModel pertPageViewModel;

        public event EventHandler CanExecuteChanged;

        public SaveAsCommand(
            AppContext appContext,
            ISaveAsBusinessLogic businessLogic,
            ProjectSettingPageViewModel projectSettingPageViewModel,
            MemberPageViewModel memberPageViewModel,
            WbsPageViewModel wbsPageViewModel,
            ProcessDependencyPageViewModel processDependencyPageViewModel,
            FunctionDependencyPageViewModel functionDependencyPageViewModel,
            PertPageViewModel pertPageViewModel,
            ShellViewModel shellViewModel,
            IMapper mapper)
        {
            this.appContext = appContext;
            this.businessLogic = businessLogic;
            this.projectSettingPageViewModel = projectSettingPageViewModel;
            this.memberPageViewModel = memberPageViewModel;
            this.wbsPageViewModel = wbsPageViewModel;
            this.processDependencyPageViewModel = processDependencyPageViewModel;
            this.functionDependencyPageViewModel = functionDependencyPageViewModel;
            this.pertPageViewModel = pertPageViewModel;
            this.shellViewModel = shellViewModel;
            this.mapper = mapper;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // ファイル保存ダイアログ
            var sfd = new SaveFileDialog();
            sfd.Filter = "Accessファイル形式|*.accdb";
            sfd.DefaultExt = ".accdb";

            var isOK = sfd.ShowDialog();

            if (isOK == false)
            {
                return;
            }

            var input = new SaveAsInput();

            input.MasterFilePath = appContext.MasterDbFile;
            input.SrcPath = appContext.ProjectDbFile;
            input.SavePath = sfd.FileName;
            input.StartDate = this.projectSettingPageViewModel.ProjectStartDate;
            input.EndDate = this.projectSettingPageViewModel.ProjectEndDate;
            input.Processes = mapper.Map<List<Process>>(this.projectSettingPageViewModel.ProcessNames);
            input.Functions = mapper.Map<List<Function>>(this.projectSettingPageViewModel.FunctionNames);
            input.Holidays = mapper.Map<List<Holiday>>(this.projectSettingPageViewModel.Holidays);
            input.RestDays = mapper.Map<List<WeekDay>>(this.projectSettingPageViewModel.Weekdays);
            input.Members = this.appContext.Members;
            input.Tasks = this.appContext.Tasks;
            input.ProcessDependencies = mapper.Map<List<ProcessDependency>>(this.processDependencyPageViewModel.Dependencies);
            input.FunctionDependencies = mapper.Map<List<FunctionDependency>>(this.functionDependencyPageViewModel.Dependencies);
            input.Edges = mapper.Map<List<Pert>>(this.pertPageViewModel.Edges);

            appContext.ProjectDbFile = input.SavePath;
            appContext.ProjectFolder = Path.GetDirectoryName(input.SavePath);

            var output = this.businessLogic.Execute(input);

            this.shellViewModel.ProjectPath = output?.ProjectPath ?? input.SrcPath;
            this.appContext.IsSaved = true;
        }
    }
}
