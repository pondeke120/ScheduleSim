using ScheduleSim.Core.Contexts;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScheduleSim.Commands.PertPage
{
    public class TaskChangeCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public TaskChangeCommand(
            AppContext appContext)
        {
            this.appContext = appContext;
        }

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            var sender = (parameter as object[])[0] as ComboBox;
            var e = (parameter as object[])[1] as SelectionChangedEventArgs;
            var viewModel = sender.DataContext as PertPageEdgeItemViewModel;
            var targetEdge = appContext.PertEdges.FirstOrDefault(x => x.Id == viewModel?.Id);

            var selectedValue = (int?)((e.OriginalSource as ComboBox).SelectedValue);
            if (targetEdge != null)
            {
                targetEdge.TaskCd = selectedValue;
                var task = appContext.Tasks.FirstOrDefault(x => x.TaskCd == targetEdge.TaskCd);
                if (task != null)
                {
                    if (viewModel.ProcessId == null)
                        viewModel.ProcessId = task.ProcessCd;
                    if (viewModel.FunctionId == null)
                        viewModel.FunctionId = task.FunctionCd;
                    viewModel.PV = task.PlanValue;
                }

                if (viewModel != null
                    && task == null
                    && e.AddedItems.Count > 0
                    && ((PertPageTaskItemViewModel)e.AddedItems[0]).TaskId == null)
                {
                    viewModel.PV = 0.0;
                }
            }

            e.Handled = true;
        }
    }
}
