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
using System.Windows;

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

            if (CheckEnable() == false)
            {
                MessageBox.Show("プロジェクト期間を入力してください。");
                return;
            }

            var input = new Core.IO.WPF.Menu.SaveInput();

            input.StartDate = this.projectSettingPageViewModel.ProjectStartDate;
            input.EndDate = this.projectSettingPageViewModel.ProjectEndDate;
            input.Processes = mapper.Map<List<Process>>(this.projectSettingPageViewModel.ProcessNames);
            input.Functions = mapper.Map<List<Function>>(this.projectSettingPageViewModel.FunctionNames);
            input.Holidays = mapper.Map<List<Holiday>>(this.projectSettingPageViewModel.Holidays);
            input.RestDays = mapper.Map<List<WeekDay>>(this.projectSettingPageViewModel.Weekdays);
            input.Members = this.appContext.Members;
            input.Tasks = this.appContext.Tasks;
            input.ProcessDependencies = this.appContext.ProcessDependencies;
            input.FunctionDependencies = this.appContext.FunctionDependencies;
            input.Edges = this.appContext.PertEdges;

            input.masterDbFile = this.appContext.MasterDbFile;
            input.currentDbFile = this.appContext.ProjectDbFile;

            this.dispatcher.Dispatch(this.businessLogic, input);
        }
        
        /// <summary>
        /// 保存可能であるか否かの判定
        /// </summary>
        /// <returns></returns>
        private bool CheckEnable()
        {
            return
                this.appContext.PrjSettings.StartDate.HasValue
                && this.appContext.PrjSettings.EndDate.HasValue;
        }
    }
}
