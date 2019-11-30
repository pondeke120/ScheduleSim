using ScheduleSim.Core.BusinessLogics.WPF.PertPage;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Core.Extensions;
using ScheduleSim.Core.IO.WPF.PertPage;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScheduleSim.Commands.PertPage
{
    public class CalcNodeNumberCommand : ICommand
    {
        private AppContext appContext;
        private ICalcNodeNumberBusinessLogic businessLogic;

        public CalcNodeNumberCommand(
            AppContext appContext,
            ICalcNodeNumberBusinessLogic businessLogic)
        {
            this.appContext = appContext;
            this.businessLogic = businessLogic;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var viewModel = parameter as PertPageViewModel;

            // ノード番号を自動割り付け
            var input = new CalcNodeNumberInput()
            {
                FunctionDependencies = this.appContext.FunctionDependencies,
                ProcessDependencies = this.appContext.ProcessDependencies,
                Tasks = viewModel.Edges.Where(x => x.TaskId.HasValue).Select(x => this.appContext.Tasks.FirstOrDefault(y => x.TaskId == y.TaskCd)).ToArray()
            };

            var ret = this.businessLogic.Execute(input);

            this.appContext.PertEdges.Clear();
            this.appContext.PertEdges.AddRange(ret.PertEdges);
        }
    }
}
