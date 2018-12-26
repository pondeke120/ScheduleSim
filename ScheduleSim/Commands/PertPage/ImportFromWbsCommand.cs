using ScheduleSim.Core.BusinessLogics.WPF.PertPage;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Core.IO.WPF.PertPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ScheduleSim.Core.Extensions;

namespace ScheduleSim.Commands.PertPage
{
    public class ImportFromWbsCommand : ICommand
    {
        private AppContext appContext;
        private IImportFromWbsBusinessLogic businessLogic;

        public event EventHandler CanExecuteChanged;

        public ImportFromWbsCommand(
            AppContext appContext,
            IImportFromWbsBusinessLogic businessLogic)
        {
            this.appContext = appContext;
            this.businessLogic = businessLogic;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var input = new ImportFromWbsInput();
            input.Tasks = this.appContext.Tasks;
            input.Perts = this.appContext.PertEdges;

            var output = this.businessLogic.Execute(input);

            this.appContext.PertEdges.Clear();
            this.appContext.PertEdges.AddRange(output.Perts);
        }
    }
}
