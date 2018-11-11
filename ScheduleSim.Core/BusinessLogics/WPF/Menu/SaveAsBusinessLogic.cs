using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSim.Core.IO.WPF.Menu;
using ScheduleSim.Core.Service;

namespace ScheduleSim.Core.BusinessLogics.WPF.Menu
{
    public class SaveAsBusinessLogic : ISaveAsBusinessLogic
    {
        private IDbFileAccessService dbFileAccessService;
        private IDbMigrationService dbMigrationService;
        private IFunctionDependencyAccessService functionDependencyAccessService;
        private IMemberAccessService memberAccessService;
        private IProcessDependencyAccessService processDependencyAccessService;
        private IProjectSettingsAccessService projectSettingsAccessService;
        private ITaskAccessService taskAccessService;

        public SaveAsBusinessLogic(
            IDbMigrationService dbMigrationService,
            IDbFileAccessService dbFileAccessService,
            IProjectSettingsAccessService projectSettingsAccessService,
            IMemberAccessService memberAccessService,
            ITaskAccessService taskAccessService,
            IProcessDependencyAccessService processDependencyAccessService,
            IFunctionDependencyAccessService functionDependencyAccessService)
        {
            this.dbMigrationService = dbMigrationService;
            this.dbFileAccessService = dbFileAccessService;
            this.projectSettingsAccessService = projectSettingsAccessService;
            this.memberAccessService = memberAccessService;
            this.taskAccessService = taskAccessService;
            this.processDependencyAccessService = processDependencyAccessService;
            this.functionDependencyAccessService = functionDependencyAccessService;
        }

        public SaveAsOutput Execute(SaveAsInput input)
        {
            var output = new SaveAsOutput();

            if (this.dbFileAccessService.Exists(input.SrcPath))
            {
                this.dbFileAccessService.CopyDbFile(input.SrcPath, input.SavePath);
            }
            else
            {
                this.dbFileAccessService.CopyDbFile(input.MasterFilePath, input.SavePath);
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

            output.ProjectPath = input.SavePath;

            return output;
        }
    }
}
