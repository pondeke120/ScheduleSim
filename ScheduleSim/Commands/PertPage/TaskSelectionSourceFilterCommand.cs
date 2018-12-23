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
    public class TaskSelectionSourceFilterCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            var cb = ((object[])parameter)[0] as ComboBox;
            var viewModel = ((object[])parameter)[1] as PertPageViewModel;
            var row = DataGridRow.GetRowContainingElement(cb);

            var edgeVm = row.Item as PertPageEdgeItemViewModel;
            if (edgeVm == null)
                return;

            var functionId = edgeVm.FunctionId;
            var processId = edgeVm.ProcessId;
            var source = viewModel.TaskSource as IEnumerable<PertPageTaskItemViewModel>;

            if (processId != null)
            {
                source = source.Where(x => x.ProcessId == processId || x.TaskId == null).ToList();
            }
            if (functionId != null)
            {
                source = source.Where(x => x.FunctionId == functionId || x.TaskId == null).ToList();
            }

            cb.ItemsSource = source;
        }
    }
}
