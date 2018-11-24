﻿using AutoMapper;
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

namespace ScheduleSim.Commands.Menu
{
    public class OpenFileCommand : ICommand
    {
        private AppContext appContext;
        private IOpenFileBusinessLogic openFileBusinessLogic;
        private ProjectSettingPageViewModel projectSettingPageViewModel;
        private MemberPageViewModel memberPageViewModel;
        private ProcessDependencyPageViewModel processDependencyPageViewModel;
        private FunctionDependencyPageViewModel functionDependencyPageViewModel;
        private WbsPageViewModel wbsPageViewModel;
        private PertPageViewModel pertPageViewModel;
        private ShellViewModel shellViewModel;
        private IMapper mapper;
        private IDispatcher dispatcher;

        public event EventHandler CanExecuteChanged;

        public OpenFileCommand(
            AppContext appContext,
            IOpenFileBusinessLogic openFileBusinessLogic,
            ProjectSettingPageViewModel projectSettingPageViewModel,
            MemberPageViewModel memberPageViewModel,
            ProcessDependencyPageViewModel processDependencyPageViewModel,
            FunctionDependencyPageViewModel functionDependencyPageViewModel,
            WbsPageViewModel wbsPageViewModel,
            PertPageViewModel pertPageViewModel,
            ShellViewModel shellViewModel,
            IMapper mapper,
            IDispatcher dispatcher)
        {
            this.appContext = appContext;
            this.openFileBusinessLogic = openFileBusinessLogic;
            this.projectSettingPageViewModel = projectSettingPageViewModel;
            this.memberPageViewModel = memberPageViewModel;
            this.processDependencyPageViewModel = processDependencyPageViewModel;
            this.functionDependencyPageViewModel = functionDependencyPageViewModel;
            this.wbsPageViewModel = wbsPageViewModel;
            this.pertPageViewModel = pertPageViewModel;
            this.shellViewModel = shellViewModel;
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
            this.projectSettingPageViewModel.ProjectStartDate = output.StartDate;
            this.projectSettingPageViewModel.ProjectEndDate = output.EndDate;

            var processNames = this.mapper.Map<List<ProjectSettingPageProcessItemViewModel>>(output.Processes);
            this.projectSettingPageViewModel.ProcessNames = Enumerable.Range(0, 20).Select(i =>
            {
                if (i < processNames.Count)
                    return processNames[i];
                return new ProjectSettingPageProcessItemViewModel();
            }).ToList();

            var functionNames = this.mapper.Map<List<ProjectSettingPageFunctionItemViewModel>>(output.Functions);
            this.projectSettingPageViewModel.FunctionNames = Enumerable.Range(0, 20).Select(i =>
            {
                if (i < functionNames.Count)
                    return functionNames[i];
                return new ProjectSettingPageFunctionItemViewModel();
            }).ToList();

            var holidays = this.mapper.Map<List<ProjectSettingPageHolidayItemViewModel>>(output.Holidays);
            this.projectSettingPageViewModel.Holidays = Enumerable.Range(0, 20).Select(i =>
            {
                if (i < holidays.Count)
                    return holidays[i];
                return new ProjectSettingPageHolidayItemViewModel();
            }).ToList();
            
            this.projectSettingPageViewModel.Weekdays = this.mapper.Map<List<ProjectSettingPageWeekdayItemViewModel>>(output.RestDays);

            this.memberPageViewModel.Members = this.mapper.Map<List<MemberPageMemberItemViewModel>>(output.Members);

            this.wbsPageViewModel.Tasks = this.mapper.Map<List<WbsPageTaskItemViewModel>>(output.Tasks);

            this.processDependencyPageViewModel.Dependencies = this.mapper.Map<List<ProcessDependencyPageDependencyItemViewModel>>(output.ProcessDependencies);
            
            this.functionDependencyPageViewModel.Dependencies = this.mapper.Map<List<FunctionDependencyPageDependencyItemViewModel>>(output.FunctionDependencies);
            
            this.pertPageViewModel.Edges = this.mapper.Map<List<PertPageEdgeItemViewModel>>(output.Edges);

            this.shellViewModel.ProjectPath = output?.FilePath ?? projectDbFileBak;
        }
    }
}
