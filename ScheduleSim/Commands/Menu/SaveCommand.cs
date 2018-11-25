using AutoMapper;
using ScheduleSim.Core.BusinessLogics.WPF.Menu;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Core.Dispatcher;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ScheduleSim.Entities.Models;

namespace ScheduleSim.Commands.Menu
{
    public class SaveCommand : ICommand
    {
        private ISaveBusinessLogic businessLogic;
        private AppContext appContext;
        private IDispatcher dispatcher;
        private ICommand saveAsCommand;
        private ProjectSettingPageViewModel projectSettingPageViewModel;
        private ShellViewModel shellViewModel;
        private MemberPageViewModel memberPageViewModel;
        private IMapper mapper;
        private WbsPageViewModel wbsPageViewModel;
        private ProcessDependencyPageViewModel processDependencyPageViewModel;
        private FunctionDependencyPageViewModel functionDependencyPageViewModel;
        private PertPageViewModel pertPageViewModel;

        public event EventHandler CanExecuteChanged;

        public SaveCommand(
            AppContext appContext,
            IDispatcher dispacher,
            ISaveBusinessLogic businessLogic,
            ICommand saveAsCommand,
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
            this.dispatcher = dispacher;
            this.businessLogic = businessLogic;
            this.saveAsCommand = saveAsCommand;
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
            if (this.appContext.IsSaved == false)
            {
                this.saveAsCommand.Execute(parameter);
                return;
            }

            var input = new Core.IO.WPF.Menu.SaveInput();

            input.StartDate = this.projectSettingPageViewModel.ProjectStartDate;
            input.EndDate = this.projectSettingPageViewModel.ProjectEndDate;
            input.Processes = mapper.Map<List<Process>>(this.projectSettingPageViewModel.ProcessNames);
            input.Functions = mapper.Map<List<Function>>(this.projectSettingPageViewModel.FunctionNames.Where(x => !string.IsNullOrEmpty(x.Name)));
            input.Holidays = mapper.Map<List<Holiday>>(this.projectSettingPageViewModel.Holidays.Where(x => x.Date.HasValue));
            input.RestDays = mapper.Map<List<WeekDay>>(this.projectSettingPageViewModel.Weekdays);
            input.Members = mapper.Map<List<Member>>(this.memberPageViewModel.Members);
            input.Tasks = mapper.Map<List<Task>>(this.wbsPageViewModel.Tasks.Where(x => !string.IsNullOrEmpty(x.TaskName)));
            input.ProcessDependencies = mapper.Map<List<ProcessDependency>>(this.processDependencyPageViewModel.Dependencies);
            input.FunctionDependencies = mapper.Map<List<FunctionDependency>>(this.functionDependencyPageViewModel.Dependencies);
            input.Edges = mapper.Map<List<Pert>>(this.pertPageViewModel.Edges);

            input.masterDbFile = this.appContext.MasterDbFile;
            input.currentDbFile = this.appContext.ProjectDbFile;

            this.dispatcher.Dispatch(this.businessLogic, input);
        }
    }
}
