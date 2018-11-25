using ScheduleSim.Core.Contexts;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScheduleSim.Commands.ProjectSettingPage
{
    public class ProcessChangeCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public ProcessChangeCommand(
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
            var sender = (parameter as object[])[0] as TextBox;
            var e = (parameter as object[])[1] as TextChangedEventArgs;
            var viewModel = sender.DataContext as ProjectSettingPageProcessItemViewModel;
            var targetProc = appContext.Processes.FirstOrDefault(x => x.ProcessCd == viewModel.Id);
            if (targetProc != null)
            {
                targetProc.ProcessName = (e.OriginalSource as TextBox).Text;
            }
            e.Handled = true;
        }
    }
}
