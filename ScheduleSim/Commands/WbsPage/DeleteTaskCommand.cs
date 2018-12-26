using ScheduleSim.Core.Contexts;
using ScheduleSim.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ScheduleSim.Core.Extensions;

namespace ScheduleSim.Commands.WbsPage
{
    public class DeleteTaskCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public DeleteTaskCommand(
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
                .Where(x => x is WbsPageTaskItemViewModel)
                .Cast<WbsPageTaskItemViewModel>().ToArray();
            var ids = viewModels.Select(x => x.No).ToArray();
            var removeTasks = this.appContext.Tasks.Where(x => ids.Contains(x.TaskCd)).ToArray();
            if (removeTasks.Length > 0)
            {
                this.appContext.Tasks.RemoveRange(removeTasks);
            }
            // 関連するPertのTaskIDをnull設定
            foreach (var pert in this.appContext.PertEdges.Where(x => x.TaskCd != null && ids.Contains(x.TaskCd.Value)))
            {
                pert.TaskCd = null;
            }
        }
    }
}
