using ScheduleSim.Core.Contexts;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ScheduleSim.Core.Extensions;
using System.Collections;
using System.Windows.Controls;

namespace ScheduleSim.Commands.PertPage
{
    public class DeleteEdgeCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public DeleteEdgeCommand(
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
            var viewModels = (parameter as IList).Cast<PertPageEdgeItemViewModel>().ToArray();
            var ids = viewModels.Select(x => x.Id).ToArray();
            var removeEdges = this.appContext.PertEdges.Where(x => ids.Contains(x.Id)).ToArray();
            if (removeEdges.Length > 0)
            {
                this.appContext.PertEdges.RemoveRange(removeEdges);
            }
        }
    }
}
