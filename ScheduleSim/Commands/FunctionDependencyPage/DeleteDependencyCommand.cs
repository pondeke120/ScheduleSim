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

namespace ScheduleSim.Commands.FunctionDependencyPage
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
                .Where(x => x is FunctionDependencyPageDependencyItemViewModel)
                .Cast<FunctionDependencyPageDependencyItemViewModel>().ToArray();
            var removeDependencies = this.appContext.FunctionDependencies.Where(x => viewModels.Any(y => y.SrcFunctionId == x.OrgFunctionCd && y.DstFunctionId == x.DstFunctionCd)).ToArray();
            if (removeDependencies.Length > 0)
            {
                this.appContext.FunctionDependencies.RemoveRange(removeDependencies);
            }
        }
    }
}
