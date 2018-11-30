using ScheduleSim.Core.Contexts;
using ScheduleSim.Entities.Enum;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScheduleSim.Commands.ProcessDependencyPage
{
    public class DependencyTypeChangeCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public DependencyTypeChangeCommand(
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
            var viewModel = sender.DataContext as ProcessDependencyPageDependencyItemViewModel;
            var targetDependency = appContext.ProcessDependencies.FirstOrDefault(x => x.OrgProcessCd == viewModel?.SrcProcessId
                                                                                        && x.DstProcessCd == viewModel?.DstProcessId);

            if (targetDependency != null)
            {
                targetDependency.DependencyType = (DependencyTypes?)sender.SelectedValue;
            }
            e.Handled = true;
        }
    }
}
