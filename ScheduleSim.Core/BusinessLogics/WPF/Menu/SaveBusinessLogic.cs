using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSim.Core.IO.WPF.Menu;
using ScheduleSim.Core.Service;

namespace ScheduleSim.Core.BusinessLogics.WPF.Menu
{
    public class SaveBusinessLogic : ISaveBusinessLogic
    {
        private IDbFileAccessService dbFileAccessService;
        private IDbMigrationService dbMigrationService;
        private IFunctionDependencyAccessService functionDependencyAccessService;
        private IMemberAccessService memberAccessService;
        private IPertAccessService pertAccessService;
        private IProcessDependencyAccessService processDependencyAccessService;
        private IProjectSettingsAccessService projectSettingsAccessService;
        private ITaskAccessService taskAccessService;

        public SaveBusinessLogic(
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

        public SaveOutput Execute(SaveInput input)
        {
            var output = new SaveOutput();

            if (this.dbFileAccessService.Exists(input.currentDbFile) == false)
            {
                this.dbFileAccessService.CopyDbFile(input.masterDbFile, input.currentDbFile);
            }

            this.dbMigrationService.Upgrade();

            this.projectSettingsAccessService.Update(
                input.StartDate,
                input.EndDate,
                input.Processes,
                input.Functions,
                input.Holidays,
                input.RestDays);

            this.memberAccessService.Update(
                input.Members);

            this.taskAccessService.Update(
                input.Tasks);

            this.processDependencyAccessService.Update(
                input.ProcessDependencies);

            this.functionDependencyAccessService.Update(
                input.FunctionDependencies);

            this.pertAccessService.Update(
                input.Edges);

            return output;
        }
    }
}
