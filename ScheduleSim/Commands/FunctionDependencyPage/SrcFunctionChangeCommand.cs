using ScheduleSim.Core.Contexts;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScheduleSim.Commands.FunctionDependencyPage
{
    public class SrcFunctionChangeCommand : ICommand
    {
        private AppContext appContext;

        public event EventHandler CanExecuteChanged;

        public SrcFunctionChangeCommand(
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
            var viewModel = sender.DataContext as FunctionDependencyPageDependencyItemViewModel;
            var model = e.AddedItems.Cast<FunctionDependencyPageFunctionItemViewModel>().FirstOrDefault();
            var targetDependency = appContext.FunctionDependencies.FirstOrDefault(x => x.OrgFunctionCd == viewModel?.SrcFunctionId
                                                                                        && x.DstFunctionCd == viewModel?.DstFunctionId);
            if (targetDependency != null
                && viewModel != null
                && model != null)
            {
                viewModel.SrcFunctionId = model.FunctionId;
                targetDependency.OrgFunctionCd = model.FunctionId;
            }
            e.Handled = true;
        }
    }
}
