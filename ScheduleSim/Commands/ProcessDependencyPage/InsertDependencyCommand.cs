using ScheduleSim.Core.Contexts;
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

namespace ScheduleSim.Commands.ProcessDependencyPage
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
            var selectedTop = viewModels.FirstOrDefault(x => x is ProcessDependencyPageDependencyItemViewModel) as ProcessDependencyPageDependencyItemViewModel;
            if (selectedTop != null)
            {
                var posItem = this.appContext.ProcessDependencies.FirstOrDefault(x => x.OrgProcessCd == selectedTop.SrcProcessId && x.DstProcessCd == selectedTop.DstProcessId);
                var index = this.appContext.ProcessDependencies.IndexOf(posItem);
                var insertDependencies = viewModels.Select(x => new ProcessDependency()
                {
                });
                this.appContext.ProcessDependencies.InsertRange(index, insertDependencies);
            }
            else
            {
                // 末尾に一つ追加
                this.appContext.ProcessDependencies.AddRange(new[] { new ProcessDependency()
                {
                }});
            }
        }
    }
}
