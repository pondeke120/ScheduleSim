using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ScheduleSim.Core.Extensions;
using System.Collections;
using ScheduleSim.ViewModels;
using ScheduleSim.Core.Contexts;

namespace ScheduleSim.Commands.ProcessDependencyPage
{
    public class DeleteDependencyCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public DeleteDependencyCommand(
            AppContext appContext)
        {
            this.appContext = appContext;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var viewModels = (parameter as IList).Cast<object>()
                .Where(x => x is ProcessDependencyPageDependencyItemViewModel)
                .Cast<ProcessDependencyPageDependencyItemViewModel>().ToArray();
            var removeDependencies = this.appContext.ProcessDependencies.Where(x => viewModels.Any(y => y.SrcProcessId == x.OrgProcessCd && y.DstProcessId == x.DstProcessCd)).ToArray();
            if (removeDependencies.Length > 0)
            {
                this.appContext.ProcessDependencies.RemoveRange(removeDependencies);
            }
        }
    }
}
