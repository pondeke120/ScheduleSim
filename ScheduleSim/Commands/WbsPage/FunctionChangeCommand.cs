using ScheduleSim.Core.Contexts;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScheduleSim.Commands.WbsPage
{
    public class FunctionChangeCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public FunctionChangeCommand(
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
            var viewModel = sender.DataContext as WbsPageTaskItemViewModel;
            var targetTask = appContext.Tasks.FirstOrDefault(x => x.TaskCd == viewModel?.No);
            if (targetTask != null)
            {
                targetTask.FunctionCd = (int)((e.OriginalSource as ComboBox).SelectedValue);
            }
            e.Handled = true;
        }
    }
}
