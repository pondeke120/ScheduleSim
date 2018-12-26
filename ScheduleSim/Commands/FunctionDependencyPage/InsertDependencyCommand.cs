using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ScheduleSim.Core.Extensions;
using System.Collections;
using ScheduleSim.ViewModels;
using ScheduleSim.Entities.Models;
using ScheduleSim.Core.Contexts;

namespace ScheduleSim.Commands.FunctionDependencyPage
{
    public class InsertDependencyCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public InsertDependencyCommand(
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
            // PertPageEdgeItemViewModel
            var viewModels = (parameter as IList).Cast<object>();
            var selectedTop = viewModels.FirstOrDefault(x => x is FunctionDependencyPageDependencyItemViewModel) as FunctionDependencyPageDependencyItemViewModel;
            if (selectedTop != null)
            {
                var posItem = this.appContext.FunctionDependencies.FirstOrDefault(x => x.OrgFunctionCd == selectedTop.SrcFunctionId && x.DstFunctionCd == selectedTop.DstFunctionId);
                var index = this.appContext.FunctionDependencies.IndexOf(posItem);
                var insertDependencies = viewModels.Select(x => new FunctionDependency()
                {
                });
                this.appContext.FunctionDependencies.InsertRange(index, insertDependencies);
            }
            else
            {
                // 末尾に一つ追加
                this.appContext.FunctionDependencies.AddRange(new[] { new FunctionDependency()
                {
                }});
            }
        }
    }
}
