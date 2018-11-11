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

        public SaveBusinessLogic(
            IDbMigrationService dbMigrationService,
            IDbFileAccessService dbFileAccessService)
        {
            this.dbMigrationService = dbMigrationService;
            this.dbFileAccessService = dbFileAccessService;
        }

        public SaveOutput Execute(SaveInput input)
        {
            var output = new SaveOutput();

            if (this.dbFileAccessService.Exists(input.currentDbFile) == false)
            {
                this.dbFileAccessService.CopyDbFile(input.masterDbFile, input.currentDbFile);
            }

            this.dbMigrationService.Upgrade();

            return output;
        }
    }
}
