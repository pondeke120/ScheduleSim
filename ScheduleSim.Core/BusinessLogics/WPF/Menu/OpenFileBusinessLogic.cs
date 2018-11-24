using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScheduleSim.Core.IO.WPF.Menu;
using ScheduleSim.Entities.Models;
using ScheduleSim.Core.Service;

namespace ScheduleSim.Core.BusinessLogics.WPF.Menu
{
    public class OpenFileBusinessLogic : IOpenFileBusinessLogic
    {
        private IDbFileAccessService dbFileAccessService;
        private IDbMigrationService dbMigrationService;
        private IFunctionDependencyAccessService functionDependencyAccessService;
        private IMemberAccessService memberAccessService;
        private IPertAccessService pertAccessService;
        private IProcessDependencyAccessService processDependencyAccessService;
        private IProjectSettingsAccessService projectSettingsAccessService;
        private ITaskAccessService taskAccessService;

        public OpenFileBusinessLogic(
            IDbMigrationService dbMigrationService,
            IDbFileAccessService dbFileAccessService,
            IProjectSettingsAccessService projectSettingsAccessService,
            IMemberAccessService memberAccessService,
            ITaskAccessService taskAccessService,
            IProcessDependencyAccessService processDependencyAccessService,
            IFunctionDependencyAccessService functionDependencyAccessService,
            IPertAccessService pertAccessService)
        {
            this.dbMigrationService = dbMigrationService;
            this.dbFileAccessService = dbFileAccessService;
            this.projectSettingsAccessService = projectSettingsAccessService;
            this.memberAccessService = memberAccessService;
            this.taskAccessService = taskAccessService;
            this.processDependencyAccessService = processDependencyAccessService;
            this.functionDependencyAccessService = functionDependencyAccessService;
            this.pertAccessService = pertAccessService;
        }

        public OpenFileOutput Execute(OpenFileInput input)
        {
            var output = new OpenFileOutput();

            output.IsError = false;
            output.FilePath = input.FilePath;

            var startDate = null as DateTime?;
            var endDate = null as DateTime?;
            var processes = null as IEnumerable<Process>;
            var functions = null as IEnumerable<Function>;
            var holidays = null as IEnumerable<Holiday>;
            var restDays = null as IEnumerable<WeekDay>;
            this.projectSettingsAccessService.GetAllSetting(
                    ref startDate,
                    ref endDate,
                    ref processes,
                    ref functions,
                    ref holidays,
                    ref restDays
                );
            output.StartDate = startDate;
            output.EndDate = endDate;
            output.Processes = processes;
            output.Functions = functions;
            output.Holidays = holidays;
            output.RestDays = restDays;
            output.Members = this.memberAccessService.GetAllMembers();
            output.ProcessDependencies = this.processDependencyAccessService.GetAllDependencies();
            output.FunctionDependencies = this.functionDependencyAccessService.GetAllDependencies();
            output.Tasks = this.taskAccessService.GetAllTasks();
            output.Edges = this.pertAccessService.GetAllEdges();

            return output;
        }
    }
}
